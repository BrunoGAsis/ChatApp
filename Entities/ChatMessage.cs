using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class ChatMessage
    {
        [Key]
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string MessageText { get; set; }
        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public bool IsChatBotMessage { get; set; }
    }
}
