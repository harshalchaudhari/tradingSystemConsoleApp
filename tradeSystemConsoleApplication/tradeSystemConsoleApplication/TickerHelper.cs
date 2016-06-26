using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket
{
    public static class TickerHelper
    {
        public static IEnumerable<string> GetTickers(string tickerFilePath, char[] delimiters)
        {
            return File.ReadAllText(tickerFilePath).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> GetConcatenatedTickers(string tickerFilePath, char[] delimiters, char separater, int maxTickersToConcatenate)
        {
            return GetConcatenatedTickers(GetTickers(tickerFilePath, delimiters), separater, maxTickersToConcatenate);
        }

        public static IEnumerable<string> GetConcatenatedTickers(IEnumerable<string> tickers, char separator, int maxTickersToConcatenate)
        {
            StringBuilder sb = new StringBuilder();
            List<string> concatenatedTickers = new List<string>();
            int count = 0;

            foreach (string ticker in tickers)
            {
                if (count++ == maxTickersToConcatenate)
                {
                    concatenatedTickers.Add(sb.ToString().TrimEnd(separator));
                    sb = new StringBuilder();
                    count = 1;
                }
                sb.Append(ticker + separator);
            }

            if (sb.Length > 0)
            {
                concatenatedTickers.Add(sb.ToString().TrimEnd(separator));
            }
            
            return concatenatedTickers;
        }
    }
}
