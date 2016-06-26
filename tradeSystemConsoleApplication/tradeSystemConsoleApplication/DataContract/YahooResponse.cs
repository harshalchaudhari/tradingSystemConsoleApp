using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tradeSystemConsoleApplication.DataContract
{
    public class YahooResponse
    {
        public List list { get; set; }
    }

    public class List
    {
        /// <summary>
        /// A collection of the User's linked accounts.
        /// </summary>
        public List<Resources> resources { get; set; }
    }

    public class Resources
    {
        public Resource resource { get; set; }
    }

    public class Resource
    {
        public Fields fields { get; set; }
    }
    public class Fields
    {
        public string price { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string volume { get; set; }
        public string chg_percent { get; set; }
    }
}
