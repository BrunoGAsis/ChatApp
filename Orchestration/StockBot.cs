using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace Orchestration
{
    public class StockBot : IStockBot
    {
        public async Task<StockResponse> GetStock(string stockCode, string url, string stockToken, HttpMethod httpMethod)
        {
            StockResponse result = new StockResponse();
            result.StockCode = stockCode;
            try
            {
                string stockUrl = url.Replace(stockToken, stockCode);
                var request = new HttpRequestMessage(httpMethod, stockUrl);
                var httpClient = new HttpClient();
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    ReadStock(data, ref result);
                }
                else
                {
                    result.Stock = string.Empty;
                    result.Error = new Exception(string.Format("Error while trying to call {0} returned status code {1}", stockUrl, response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                result.Stock = string.Empty;
                result.Error = ex;
            }

            return result;
        }

        private bool ReadStock(Stream csvData, ref StockResponse result)
        {
            try
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                CSVStockInfoMapping csvMapper = new CSVStockInfoMapping();
                CsvParser<StockInfo> csvParser = new CsvParser<StockInfo>(csvParserOptions, csvMapper);
                var csvRows = csvParser.ReadFromStream(csvData,Encoding.UTF8).GetEnumerator();
                csvRows.MoveNext();

                if (csvRows.Current.Result == null)
                    throw new Exception("Invalid file format");

                double close = 0;

                if(double.TryParse(csvRows.Current.Result.Close, out close))
                {
                    result.Stock = csvRows.Current.Result.Close;
                    result.Error = null;
                }
                else
                {
                    result.Stock = string.Empty;
                    result.Error = new Exception("Error while trying to read stock value from endpoint file");
                }
                return (result.Stock != string.Empty);
            }
            catch (Exception ex)
            {
                result.Stock = string.Empty;
                result.Error = ex;
                return false;
            }

        }
    }
}
