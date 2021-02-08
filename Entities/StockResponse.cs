using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class StockResponse
    {
        public string Stock { get; set; }
        public Exception Error { get; set; }
    }
}
