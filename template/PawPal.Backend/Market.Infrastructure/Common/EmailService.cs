using Azure.Core;
using Microsoft.Extensions.Configuration;
using PawPal.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Infrastructure.Common
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);
            var fromEmail = _config["Smtp:FromEmail"];
            var fromName = _config["Smtp:FromName"];

            var client = new SmtpClient(host, port)
            {
                EnableSsl = false, // Mailpit doesn't need SSL
                Credentials = CredentialCache.DefaultNetworkCredentials
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);

            await client.SendMailAsync(message);
        }
    }
}
