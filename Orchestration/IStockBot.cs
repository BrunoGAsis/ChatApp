using Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration
{
    public interface IStockBot
    {
        Task<StockResponse> GetStock(string stockCode, string url, string stockToken, HttpMethod httpMethod);
    }
}
