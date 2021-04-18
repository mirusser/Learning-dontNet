using EmailDemo.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailDemo.Services
{
    public interface IMailService
    {
        Task SendStandardEmailAsync(MailRequest request);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
        Task SendEmailAsync(MimeMessage email);
    }
}
