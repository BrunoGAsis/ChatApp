using Business;
using Data;
using Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orchestration
{
    public class MessageController : Hub, IMessageController
    {
        private readonly IStockBot _stockBot;
        private readonly IMessageManager _manager;
        private readonly IChatMessageBusiness _chatMessageBusiness;


        public MessageController(IStockBot stockBot, IMessageManager manager, IChatMessageBusiness chatMessageBusiness)
        {
            _stockBot = stockBot;
            _manager = manager;
            _chatMessageBusiness = chatMessageBusiness;
        }



        public async Task SendMessage(ChatMessage message)
        {

            if (message.MessageText.StartsWith(OrchestrationConfiguration.StockCommandToken, StringComparison.InvariantCultureIgnoreCase))
            {
                string stockCode = message.MessageText.Substring(7);
                await _stockBot.GetStock(stockCode, OrchestrationConfiguration.BotStockEndPoint, OrchestrationConfiguration.BotStockCodeToken, OrchestrationConfiguration.BotHTTPMethod, true, this, message.UserEmail);
            }
            else
            {
                List<Guid> removedIds = null;
                if (_manager.AddMessage(message, out removedIds))
                {
                    try
                    {
                        message = _chatMessageBusiness.Add(message);
                        await Clients.All.SendAsync("addMessage", message);
                        await RemoveMessages(removedIds);
                    }
                    catch(Exception)
                    {
                        ChatMessage error = new ChatMessage();
                        error.MessageText = "Error while tryng to send your message, please try again";
                        await SendErrorMessage(error);
                    }
                }
            }
        }


        public async Task RemoveMessages(List<Guid> ids)
        {
            foreach (Guid messageId in ids)
            {
                await Clients.All.SendAsync("removeMessage", messageId);
            }
        }


        public async Task SendErrorMessage(ChatMessage message)
        {
            //Failiure message only for caller, not added to general queue
            await Clients.Caller.SendAsync("addErrorMessage", message);
        }
    }
}
