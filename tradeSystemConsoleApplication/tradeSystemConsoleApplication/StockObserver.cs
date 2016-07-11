using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tradeSystemConsoleApplication;
using tradeSystemConsoleApplication.DataContract;
using tradeSystemConsoleApplication.NotificationManager;
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
        public string Recepients { get; private set; }

        public StockObserver(
            IEnumerable<string> concatenatedTickers, 
            string concatenatedRecepients, 
            double triggerThreshold, 
            int maximumCachedValues, 
            int minimumVolumeTradedInDayThreshold)
        {
            cachedValues = new Queue<Dictionary<string, Fields>>();
            MaximumCachedValues = maximumCachedValues;
            MinimumVolumeTradedInDayThreshold = minimumVolumeTradedInDayThreshold;
            ConcatenatedTickers = concatenatedTickers;
            TriggerThreshold = triggerThreshold;
            Recepients = concatenatedRecepients;
        }

        public List<KeyValuePair<string, double>> GetInterestingStocks(Dictionary<string, Fields> oldestStockQuotes, Dictionary<string, Fields> latestStockQuotes)
        {
            Dictionary<string, double> difference = GetDifference(oldestStockQuotes, latestStockQuotes);

            return difference.Where(stock => stock.Value >= TriggerThreshold
                                             && Int32.Parse(latestStockQuotes[stock.Key].volume) >= MinimumVolumeTradedInDayThreshold).ToList();
        }

        public void Observe()
        {
            Trace.TraceInformation("Getting latest stock information...");
            Dictionary<string, Fields> latestStockQuotes = GetLatestStockActivity(ConcatenatedTickers);

            if (cachedValues.Count != 0)
            {
                Dictionary<string, Fields> oldestStockQuotes = cachedValues.Peek();

                Trace.TraceInformation("Comparing oldest stock quotes with latest stock quotes");
                List<KeyValuePair<string, double>> interestingStocks = GetInterestingStocks(oldestStockQuotes, latestStockQuotes);

                if (interestingStocks.Count > 0)
                {
                    Notify(interestingStocks, Recepients);
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

        public static string ConstructEmailContent(List<KeyValuePair<string, double>> stockDifference)
        {
            StringBuilder htmlBodyContent = new StringBuilder();

            htmlBodyContent.Append("<HTML>");
            htmlBodyContent.Append("<Body>");
            htmlBodyContent.Append("<Table border=1>");
            htmlBodyContent.Append("<TR><TH>Symbol</TH><TH>Price Difference in %</TH></TR>");

            foreach (KeyValuePair<string, double> currentDifference in stockDifference)
            {
                htmlBodyContent.Append(string.Format("<TR><TD>{0}</TD>{1}</TR>", currentDifference.Key, currentDifference.Value));
            }

            htmlBodyContent.Append("</Table>");
            htmlBodyContent.Append("</Body>");
            htmlBodyContent.Append("</HTML>");

            return htmlBodyContent.ToString();
        }

        public static void Notify(List<KeyValuePair<string, double>> stockDifference, string concatenatedRecepients)
        {
            Trace.TraceInformation("{0} stocks exceeded given threshold!!", stockDifference.Count);
            Trace.TraceInformation("Time to notify: {0}", DateTime.Now);
            
            string body = ConstructEmailContent(stockDifference);
            Trace.TraceInformation("Content: {0}", body);

            try
            {
                EmailManager.SendEmail(body, concatenatedRecepients, true);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Ran into exception while sending email! Error: {0}", exception);
            }
            
        }
    }
}
