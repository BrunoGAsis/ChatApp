using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Orchestration
{
    public static class OrchestrationConfiguration
    {
        //Default values
        private static string _botStockEndpoint = "https://stooq.com/q/l/?s=[stockCode]&f=sd2t2ohlcv&h&e=csv​";
        private static string _botStockCodeToken = "[stockCode]";
        private static int  _messageManagerQueueLimit = 50;
        private static string _commandToken = "/stock=";
        private static HttpMethod _botHTTPMethod = HttpMethod.Get;
        public static string BotStockEndPoint 
        {
            get
            {
                return _botStockEndpoint;
            }
            set
            {
                _botStockEndpoint = value;
            }
        }
        public static string BotStockCodeToken 
        { 
            get
            {
                return _botStockCodeToken;
            }
            set
            {
                _botStockCodeToken = value;
            }
        }

        public static int MessageManagerQueueLimit
        {
            get
            {
                return _messageManagerQueueLimit;
            }
            set
            {
                _messageManagerQueueLimit = value;
            }
        }

        public static string CommandToken
        {
            get
            {
                return _commandToken;
            }
            set
            {
                _commandToken = value;
            }
        }

        public static HttpMethod BotHTTPMethod
        {
            get
            {
                return _botHTTPMethod;
            }
            set
            {
                _botHTTPMethod = value;
            }
        }
    }
}
