using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace ALlyHub
{
    public class SendMessage
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new ArgumentNullException(nameof(toEmail), "Recipient email cannot be null or empty.");
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("", ""), //UserName : allyhub324047@gmail.com   Pass:qqoy vkmz cvvf xmgg 
                EnableSsl = true,
            };

            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new MailAddress("allyhub324047@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

    }
}
