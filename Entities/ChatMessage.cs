using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
