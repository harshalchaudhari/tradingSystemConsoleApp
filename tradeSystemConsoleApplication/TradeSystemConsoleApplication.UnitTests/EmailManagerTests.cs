using System;
using System.Text;
using System.Collections.Generic;
using tradeSystemConsoleApplication.NotificationManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradeSystemConsoleApplication.UnitTests
{
    /// <summary>
    /// Summary description for EmailManagerTests
    /// </summary>
    [TestClass]
    public class EmailManagerTests
    {
        [TestMethod]
        public void SendEmailAlerts_ValidData_Success()
        {
            string message = @"<html><body><table><li>Kunal</li><li>Harshal</li><li>Rushabh</li><li>Rahul</li><li>Ramchandra</li></table></body></html>";
            string recepients = "kp101090@gmail.com";

            EmailManager.sendEmailAlert(message, recepients, true);
        }
    }
}
