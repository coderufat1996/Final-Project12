using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineShoppingApp.Options;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Servıces
{
    public class EmailService : IEmailService
    {
       
        private EmailConfigOptions _emailConfig;

        public EmailService(IOptions<EmailConfigOptions> emailConfigOptions)
        {
            _emailConfig = emailConfigOptions.Value;
        }

        public void Send(MessageViewModel message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var body = new BodyBuilder();

            body.HtmlBody = message.Content;
            emailMessage.Body = body.ToMessageBody();

            SendMessage(emailMessage);
        }


        private void SendMessage(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.From, _emailConfig.Password);
                    client.Send(emailMessage);
                }
                catch
                {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);

                    client.Dispose();
                }
            }
        }
    }

}
