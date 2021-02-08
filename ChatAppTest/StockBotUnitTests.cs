using Orchestration;
using System;
using System.Net.Http;
using Xunit;

namespace ChatAppTest
{
    public class StockBotUnitTests
    {
        private readonly string botUrl = "https://stooq.com/q/l/?s=[stockcode]&f=sd2t2ohlcv&h&e=csv";
        private readonly string stockCodeToken = "[stockcode]";
        [Fact]
        public void GetStock()
        {
            StockBot bot = new StockBot();
            string stockCode = "aapl.us";
            var StockResponse = bot.GetStock(stockCode, botUrl, stockCodeToken, HttpMethod.Get).Result;

            Assert.True(StockResponse.Stock != string.Empty);
        }

        [Fact]
        public void GetStockFail()
        {
            StockBot bot = new StockBot();
            string stockCode = "NotExists";

            var StockResponse = bot.GetStock(stockCode, botUrl, stockCodeToken, HttpMethod.Get).Result;

            Assert.True(StockResponse.Stock == string.Empty);
        }

        [Fact]
        public void GetStockBadUrl()
        {
            StockBot bot = new StockBot();
            string stockCode = "NOTEXISTS";

            var StockResponse = bot.GetStock(stockCode, "http://www.microsoft.com/", stockCodeToken, HttpMethod.Get).Result;

            Assert.NotNull(StockResponse.Error);
        }
    }
}
