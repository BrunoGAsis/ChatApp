using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class ChatMessageData : IChatMessageData
    {
        ApplicationDbContext _dbContext;
        public ChatMessageData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ChatMessage Add(ChatMessage msg)
        {
            _dbContext.ChatMessage.Add(msg);
            _dbContext.SaveChanges();
            return msg;
        }

        public List<ChatMessage> GetLastMessages(int count)
        {
            return _dbContext.ChatMessage.AsQueryable().OrderByDescending( r=> r.CreatedDate).Take(count).ToList();
        }
    }
}
