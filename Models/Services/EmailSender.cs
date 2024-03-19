using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineShop.Models.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nkduy.dev@gmail.com", "yphglldxtebyacig")
            };

            MailMessage mail = new MailMessage("nkduy.dev@gmail.com", email)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage
            };

            Console.WriteLine("Sent Email");
            return client.SendMailAsync(mail);
        }
    }
}
