using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StockResponse
    {
        public string StockCode { get; set; }
        public string StockValue { get; set; }
        public Exception Error { get; set; }
    }
}
