using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IChatMessageBusiness
    {
        public ChatMessage Add(ChatMessage msg);

        public List<ChatMessage> GetLastMessages(int count);

    }
}
