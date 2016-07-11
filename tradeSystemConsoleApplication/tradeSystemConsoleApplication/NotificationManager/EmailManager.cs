using System;
using System.Net;
using System.Net.Mail;

namespace tradeSystemConsoleApplication.NotificationManager
{
    public class EmailManager
    {
        public static void SendEmail(string body, string concatenatedRecepients, bool isBodyHtml = false)
        {
            using (MailMessage msg = new MailMessage())
            using (SmtpClient client = new SmtpClient())
            {
                msg.From = new MailAddress("YourMail@gmail.com");
                msg.To.Add(string.Join(",", concatenatedRecepients.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)));
                msg.Subject = "TradeSystem notification:" + DateTime.Now.ToString();
                msg.Body = body;
                msg.IsBodyHtml = isBodyHtml;

                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("chhapparPhadke@gmail.com", "SubjectToMarketRisk");
                client.Timeout = 20000;

                client.Send(msg);
            }
        }
    }
}
