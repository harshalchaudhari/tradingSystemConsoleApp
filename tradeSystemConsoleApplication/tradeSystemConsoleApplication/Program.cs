using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tradeSystemConsoleApplication.DataContract;

namespace tradeSystemConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Fields> dict = YahooApi.yahooGetQuotes("AAPL,YHOO",true);
            Dictionary<string, Fields> dict1 = YahooApi.yahooGetQuotes("AAPL,YHOO");
        }
    }
}
