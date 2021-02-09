using Business;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Orchestration
{
    public class MessageManager : IMessageManager
    {
        private ConcurrentQueue<ChatMessage> _availableMessages = new ConcurrentQueue<ChatMessage>();
        private int _messageId = 0;
        private object _lockObj = new object();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MessageManager(IServiceScopeFactory serviceScopeFactory)
        {
            if (OrchestrationConfiguration.RetrieveHistoryChatOnStartup)
            {
                _serviceScopeFactory = serviceScopeFactory;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IChatMessageBusiness msgBss = scope.ServiceProvider.GetService<IChatMessageBusiness>();
                    StoreMessages(msgBss.GetLastMessages(OrchestrationConfiguration.MessageManagerQueueLimit));
                }
            }
        }

        private void StoreMessages(List<ChatMessage> messages)
        {
            foreach (ChatMessage msg in messages)
                _availableMessages.Enqueue(msg);
        }


        public bool AddMessage(ChatMessage message, out List<Guid> removedMessageIds)
        {
            if (string.IsNullOrWhiteSpace(message.MessageText))
            {
                removedMessageIds = new List<Guid>();
                return false;
            }

            lock (_lockObj)
            {
                AddToAvailable(message, out removedMessageIds);
            }
            return true;
        }

        private int GetMessageId()
        {
            _messageId++;
            return _messageId;
        }

        private void AddToAvailable(ChatMessage message, out List<Guid> removedMessageIds)
        {
            ChatMessage msg = null;
            _availableMessages.Enqueue(message);

            removedMessageIds = new List<Guid>();
            while (_availableMessages.Count > OrchestrationConfiguration.MessageManagerQueueLimit)
            {
                if (_availableMessages.TryDequeue(out msg))
                {
                    removedMessageIds.Add(msg.Id);
                }
            }
        }


        public ChatMessage[] GetAvailableMessages()
        {
            return _availableMessages.ToArray();
        }

    }
}
