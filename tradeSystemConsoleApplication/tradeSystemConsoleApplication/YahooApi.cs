using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using tradeSystemConsoleApplication.DataContract;

namespace tradeSystemConsoleApplication
{
    public class YahooDataObject
    {
        public string Name { get; set; }
    }
    public class YahooApi
    {
        
        public static Dictionary<string, Fields> yahooGetQuotes(string tickrString, bool getDetails=false, string responseFormat = "json")
        {
            if (String.IsNullOrEmpty(tickrString))
            {
                return null;
            }
            string getQuotesPrefix = "http://finance.yahoo.com/webservice/v1/symbols/"; 
            string getQuotesSuffix = "/quote?format=";
            string url = getQuotesPrefix + tickrString + getQuotesSuffix + responseFormat;
            if(getDetails == true)
            {
                url = url + "&view=detail";
            }
            string response = callGetApi(url);
            YahooResponse yahooResponse = JsonConvert.DeserializeObject<YahooResponse>(response);
            Dictionary<string, Fields> responseDict = new Dictionary<string, Fields>();
            foreach (Resources resources in yahooResponse.list.resources)
            {
                responseDict.Add(resources.resource.fields.symbol, resources.resource.fields);
            }
            

            return responseDict;
        }

        private static string callGetApi(string url)
        {
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);
                string response = client.DownloadString(uri);
                return response;
            }
        }
    }
}
