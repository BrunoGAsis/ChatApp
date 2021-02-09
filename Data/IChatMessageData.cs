using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IChatMessageData
    {
        public ChatMessage Add(ChatMessage msg);
        public List<ChatMessage> GetLastMessages(int count);
    }
}
