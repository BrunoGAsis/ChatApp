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

        public async Task<StockResponse> GetStock(string stockCode, string url, string stockToken, HttpMethod httpMethod, bool sendMessage, IMessageController msgController, string userEmail)
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
                    result.StockValue = string.Empty;
                    result.Error = new Exception(string.Format("Error while trying to call {0} returned status code {1}", stockUrl, response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                result.StockValue = string.Empty;
                result.Error = ex;
            }

            if(sendMessage)
            {
                ChatMessage msg = new ChatMessage();
                msg.UserEmail = "ChatBot";
                msg.IsChatBotMessage = true;
                if (result.StockValue != string.Empty)
                {
                    msg.MessageText = string.Format("{0} quote is ${1} per share", stockCode, result.StockValue);
                    
                    await msgController.SendMessage(msg);
                }
                else
                {
                    msg.MessageText = string.Format("Sorry {0}, I could not retrieve the quote share for {1}", userEmail, stockCode);
                    if(result.Error != null)
                    {
                        msg.MessageText += string.Format(", reason: {0}", result.Error.Message);
                    }
                    await msgController.SendErrorMessage(msg);
                }
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
                    result.StockValue = csvRows.Current.Result.Close;
                    result.Error = null;
                }
                else
                {
                    result.StockValue = string.Empty;
                    result.Error = new Exception("Error while trying to read stock value, as it is not available");
                }
                return (result.StockValue != string.Empty);
            }
            catch (Exception ex)
            {
                result.StockValue = string.Empty;
                result.Error = ex;
                return false;
            }

        }
    }
}
