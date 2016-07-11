using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tradeSystemConsoleApplication.DataContract;
using tradeSystemConsoleApplication.NotificationManager;
using System.Diagnostics;

namespace tradeSystemConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Fields> dict = YahooApi.yahooGetQuotes("AAPL,YHOO",true);
            Dictionary<string, Fields> dict1 = YahooApi.yahooGetQuotes("AAPL,YHOO");
            Trace.TraceInformation("Starting Stock observing application!");
            //Console.WriteLine(EmailManager.SendEmailAlert("This means emailNotificationManager is working in tradeSystm","harshal420@gmail.com;rushabhmehta05@gmail.com;kp101090@gmail.com"));
            //Implement log to log emails sent, triggers etc
        }
    }
}
