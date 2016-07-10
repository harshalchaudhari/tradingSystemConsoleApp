using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace tradeSystemConsoleApplication.NotificationManager
{
    public class EmailManager
    {
        public static string sendEmailAlert(string messageContent, string toEmailAddresses, bool isBodyHtml = false)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("YourMail@gmail.com");
            foreach (string address in toEmailAddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                msg.To.Add(new MailAddress(address));
            }
            msg.Subject = "TradeSystem notification:" + DateTime.Now.ToString();
            msg.Body = messageContent;
            msg.IsBodyHtml = isBodyHtml;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("chhapparPhadke@gmail.com", "SubjectToMarketRisk");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }
    }
}
