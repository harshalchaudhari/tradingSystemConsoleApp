using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tradeSystemConsoleApplication;
using tradeSystemConsoleApplication.DataContract;
using System.Diagnostics.Tracing;
using System.Diagnostics;

namespace StockMarket
{
    public class StockObserver
    {
        private readonly Queue<Dictionary<string, Fields>> cachedValues;

        public int MaximumCachedValues { get; private set; }
        public int MinimumVolumeTradedInDayThreshold { get; private set; }
        public IEnumerable<string> ConcatenatedTickers { get; private set; }
        public double TriggerThreshold { get; private set; }

        //TODO: Fix the hard coded values
        public StockObserver(IEnumerable<string> concatenatedTickers, double triggerThreshold) : this(concatenatedTickers, triggerThreshold, 5, 100000)
        { }

        public StockObserver(IEnumerable<string> concatenatedTickers, double triggerThreshold, int maximumCachedValues, int minimumVolumeTradedInDayThreshold)
        {
            cachedValues = new Queue<Dictionary<string, Fields>>();
            MaximumCachedValues = maximumCachedValues;
            MinimumVolumeTradedInDayThreshold = minimumVolumeTradedInDayThreshold;
            ConcatenatedTickers = concatenatedTickers;
            TriggerThreshold = triggerThreshold;
        }

        public List<KeyValuePair<string, double>> GetInterestingStocks(Dictionary<string, Fields> oldestStockQuotes, Dictionary<string, Fields> latestStockQuotes)
        {
            Dictionary<string, double> difference = GetDifference(oldestStockQuotes, latestStockQuotes);

            return difference.Where(stock => stock.Value >= TriggerThreshold
                                             && Int32.Parse(latestStockQuotes[stock.Key].volume) >= MinimumVolumeTradedInDayThreshold).ToList();
        }

        public void Observe()
        {
            Dictionary<string, Fields> latestStockQuotes = GetLatestStockActivity(ConcatenatedTickers);

            if (cachedValues.Count != 0)
            {
                Dictionary<string, Fields> oldestStockQuotes = cachedValues.Peek();
                
                List<KeyValuePair<string, double>> interestingStocks = GetInterestingStocks(oldestStockQuotes, latestStockQuotes);

                if (interestingStocks.Count > 0)
                {
                    //TODO: TryNotify
                    
                }

                if (cachedValues.Count >= MaximumCachedValues)
                {
                    cachedValues.Dequeue();
                }
            }
            cachedValues.Enqueue(latestStockQuotes);
        }

        public static Dictionary<string, Fields> GetLatestStockActivity(IEnumerable<string> concatenatedTickers)
        {
            Dictionary<string, Fields> latestStockQuotes = new Dictionary<string, Fields>();

            foreach (string concatenatedTicker in concatenatedTickers)
            {
                var dictionary = YahooApi.yahooGetQuotes(concatenatedTicker);

                foreach (KeyValuePair<string, Fields> quote in dictionary)
                {
                    latestStockQuotes[quote.Key] = quote.Value;
                }
            }

            return latestStockQuotes;
        }

        public static Dictionary<string, double> GetDifference(Dictionary<string, Fields> oldestStockQuotes, Dictionary<string, Fields> latestStockQuotes)
        {
            Dictionary<string, double> difference = new Dictionary<string, double>();

            foreach (KeyValuePair<string, Fields> ticker in oldestStockQuotes)
            {
                double oldValue = Double.Parse(ticker.Value.price);
                double newValue = Double.Parse(latestStockQuotes[ticker.Key].price);

                double percentChange = ((newValue - oldValue) * 100) / oldValue;
                difference[ticker.Key] = percentChange;
            }

            return difference;
        }

        public static string ConstructEmailContent(Dictionary<string, double> stockDifferences)
        {
            return string.Empty;
        }
    }
}
