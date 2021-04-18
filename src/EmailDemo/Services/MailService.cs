using EmailDemo.Models;
using EmailDemo.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailDemo.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        private async Task<MimeMessage> CreateStandardMimeMessage(MailRequest mailRequest)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject
            };

            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            var builder = new BodyBuilder();

            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;

                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.Name, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            return email;
        }

        private async Task<MimeMessage> CreateWelcomeMimeMessage(WelcomeRequest request)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader streamReader = new (filePath);
            var mailText = streamReader.ReadToEnd();
            streamReader.Close();

            mailText = mailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail)
            };

            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";

            var builder = new BodyBuilder
            {
                HtmlBody = mailText
            };

            email.Body = builder.ToMessageBody();

            return email;
        }

        public async Task SendStandardEmailAsync(MailRequest request)
        {
            var welcomeEmail = await CreateStandardMimeMessage(request);
            await SendEmailAsync(welcomeEmail);
        }

        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            var welcomeEmail = await CreateWelcomeMimeMessage(request);
            await SendEmailAsync(welcomeEmail);
        }

        public async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
