using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration
{
    public interface IMessageController
    {
        Task SendMessage(ChatMessage message);

        Task SendErrorMessage(ChatMessage message);

    }
}
