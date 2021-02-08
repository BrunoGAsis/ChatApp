using Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Orchestration
{
    public class MessageManager : Hub
    {
        private readonly IStockBot _stockBot;
        public MessageManager(IStockBot stockBot)
        {
            _stockBot = stockBot;
        }
        public async Task SendMessage(ChatMessage message)
        {
            if (message.MessageText.ToLower().StartsWith("/stock="))
            {
                //TODO: call stock bot
            }
            else
                await Clients.All.SendAsync("addMessage", message);
        }

        public async Task RemoveMessage(int messageId)
        {
            await Clients.All.SendAsync("removeMessage", messageId);
        }

    }
}
