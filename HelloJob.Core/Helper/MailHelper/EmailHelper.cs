using HelloJob.Core.Configuration.Abstract;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text.RegularExpressions;

namespace HelloJob.Core.Helper.MailHelper
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailHelper(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new(pattern);
            return regex.IsMatch(email);
        }

        public async Task<IResult> SendEmailAsync(string email, string name,string subject, string body, string token)
        {
            try
            {
                string senderEmail = _emailConfiguration.Email;
                string senderPassword = _emailConfiguration.Password;
                int port = _emailConfiguration.Port;
                string smtp = _emailConfiguration.SmtpServer;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("HelloJob",senderEmail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Importance = MessageImportance.High;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtp, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return new SuccessResult("Email send succesfully!");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }


    }
}
