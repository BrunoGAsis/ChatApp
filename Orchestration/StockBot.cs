using Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration
{
    public class StockBot : IStockBot
    {
        IHttpClientFactory _clientFactory;

       public StockBot(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public Task<StockResponse> getStock(string stockCode)
        {
            try
            {
                string stockUrl = OrchestrationConfiguration.BotStockEndPoint.Replace(OrchestrationConfiguration.BotStockCodeToken, stockCode);
                HttpRequestMessage request = new HttpRequestMessage(OrchestrationConfiguration.BotHTTPMethod, stockUrl);
                _clientFactory.CreateClient()
            }
            catch(Exception ex)
            {

            }
        }
    }
}
