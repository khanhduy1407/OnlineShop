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
                Credentials = new NetworkCredential("kn145660@gmail.com", "jxablhrupqizieyb")
            };

            MailMessage mail = new MailMessage("kn14560@gmail.com", email)
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
