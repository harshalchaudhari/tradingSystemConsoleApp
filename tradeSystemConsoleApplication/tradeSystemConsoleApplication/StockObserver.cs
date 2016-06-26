using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket
{
    public class StockObserver
    {
        private readonly Queue<Dictionary<string, object>> cachedValues;

        public int MaximumCachedValues { get; private set; }
        public int MinimumVolumeTradedInDayThreshold { get; private set; }

        //TODO: Fix the hard coded values
        public StockObserver() : this(5, 100000)
        {}

        public StockObserver(int maximumCachedValues, int minimumVolumeTradedInDayThreshold)
        {
            cachedValues = new Queue<Dictionary<string, object>>();
            MaximumCachedValues = maximumCachedValues;
            MinimumVolumeTradedInDayThreshold = minimumVolumeTradedInDayThreshold;
        }
    }
}
