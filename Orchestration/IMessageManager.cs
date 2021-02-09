using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orchestration
{
    public interface IMessageManager
    {

        public bool AddMessage(ChatMessage message, out List<Guid> removedMessageIds);

        public ChatMessage[] GetAvailableMessages();

    }
}
