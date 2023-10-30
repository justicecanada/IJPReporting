using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IJPReporting.Helpers
{
    public class Mailer
    {
        public Mailer() { }

        public int SendMail(string subject, string body, string from, string to)
        {
            int result = 1;
            try
            {
                MailMessage mail = new MailMessage(from, to);
                SmtpClient client = new SmtpClient
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = ConfigurationManager.AppSettings["emailServiceSetting"]
                };
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
            }
            catch
            {
                result = -1;
            }

            return result;
        }
    }
}