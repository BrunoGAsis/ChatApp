using Data;
using Entities;
using System;
using System.Collections.Generic;

namespace Business
{
    public class ChatMessageBusiness: IChatMessageBusiness
    {
        private readonly IChatMessageData _chatMessageData;

        public ChatMessageBusiness(IChatMessageData chatMessageData)
        {
            _chatMessageData = chatMessageData;
        }

        public ChatMessage Add(ChatMessage msg)
        {
            if (string.IsNullOrWhiteSpace(msg.MessageText))
                throw new Exception("Can't save empty chat message");

            msg.Id = Guid.NewGuid();
            msg.CreatedDate = DateTime.Now;

            if (msg.IsChatBotMessage)
                return msg;

            return _chatMessageData.Add(msg);
        }

        public List<ChatMessage> GetLastMessages(int count)
        {
            return _chatMessageData.GetLastMessages(count);
        }
    }
}
