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
            IEnumerable<string> tickers = TickerHelper.GetTickers(@"C:\StockData\SymbolsTop100.csv", new char[]{'\n', '\r'});
            StockObserver stockObserver = new StockObserver(tickers, 10.0);

            for (int i=0; i<15;i++)
            {
                stockObserver.Observe();
                Thread.Sleep(5000);
            }
        }
    }
}
