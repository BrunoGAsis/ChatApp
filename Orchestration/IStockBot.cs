using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration
{
    public interface IStockBot
    {
        async Task<StockResponse> getStock(string stockCode);
    }
}
