using Orchestration;
using System;
using System.Net.Http;
using Xunit;
using Moq;
using Data;
namespace ChatAppTest
{
    public class StockBotUnitTests
    {
        private readonly string botUrl = "https://stooq.com/q/l/?s=[stockcode]&f=sd2t2ohlcv&h&e=csv";
        private readonly string stockCodeToken = "[stockcode]";
        private readonly string testEmail = "test@email.com";
        [Fact]
        public void GetStockFine()
        {
            StockBot bot = new StockBot();
            string stockCode = "aapl.us";

            var mockMsgController = new Mock<IMessageController>();

            var StockResponse = bot.GetStock(stockCode, botUrl, stockCodeToken, HttpMethod.Get, true, mockMsgController.Object, testEmail).Result;

            Assert.True(StockResponse.StockValue != string.Empty);
        }

        [Fact]
        public void GetStockFail()
        {
            StockBot bot = new StockBot();
            string stockCode = "NotExists";

            var mockMsgController = new Mock<IMessageController>();

            var StockResponse = bot.GetStock(stockCode, botUrl, stockCodeToken, HttpMethod.Get, true,mockMsgController.Object, testEmail).Result;

            Assert.True(StockResponse.StockValue == string.Empty);
        }

        [Fact]
        public void GetStockBadUrl()
        {
            StockBot bot = new StockBot();
            string stockCode = "NOTEXISTS";

            var mockMsgController = new Mock<IMessageController>();

            var StockResponse = bot.GetStock(stockCode, "http://www.microsoft.com/", stockCodeToken, HttpMethod.Get, true, mockMsgController.Object, testEmail).Result;

            Assert.NotNull(StockResponse.Error);
        }

        [Fact]
        public void TestDB()
        {
            
            
        }
    }
}
