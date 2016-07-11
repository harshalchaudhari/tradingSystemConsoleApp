using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockMarket;
using System.Collections.Generic;
using System.Threading;

namespace TradeSystemConsoleApplication.UnitTests
{
    [TestClass]
    public class StockObserverUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IEnumerable<string> tickers = TickerHelper.GetConcatenatedTickers(@"C:\StockData\SymbolsTop100.csv", new char[]{'\n', '\r'}, ',', 5);
            StockObserver stockObserver = new StockObserver(tickers, "kp101090@gmail.com", 0.0, 5, 1);

            for (int i=0; i<2;i++)
            {
                stockObserver.Observe();
                Thread.Sleep(5000);
            }
        }
    }
}
