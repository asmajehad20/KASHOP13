using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("asmajehad919@gmail.com", "ttff rljw fioc uiyk")
            };

            return client.SendMailAsync(
                new MailMessage(from: "asmajehad919@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml = true});
        }
    }
}
