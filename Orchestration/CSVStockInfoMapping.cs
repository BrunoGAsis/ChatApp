using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.Mapping;

namespace Orchestration
{
    internal class CSVStockInfoMapping : CsvMapping<StockInfo>
    {
        public CSVStockInfoMapping(): base()
        {
            MapProperty(0, c => c.Symbol);
            MapProperty(1, c => c.Date);
            MapProperty(2, c => c.Time);
            MapProperty(3, c => c.Open);
            MapProperty(4, c => c.High);
            MapProperty(5, c => c.Low);
            MapProperty(6, c => c.Close);
            MapProperty(7, c => c.Volume);
        }
    }
}
