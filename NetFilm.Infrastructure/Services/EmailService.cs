using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NetFilm.Application.Interfaces;

namespace NetFilm.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(string receptor, string subject, string body)
        {
            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, receptor, subject, body);
            await smtpClient.SendMailAsync(message);
        }

        public async Task SendEmailOtp(string receptor, string subject, string otp)
        {
            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var bodyHtml = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Your OTP Code</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap');
    </style>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Inter', Arial, sans-serif; background-color: #09090b;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #09090b; padding: 20px;"">
        <tr>
            <td align=""center"" style=""padding: 20px;"">
                <table role=""presentation"" style=""max-width: 600px; width: 100%; border-collapse: collapse; background-color: #18181b; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);"">
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 30px 40px; text-align: center; border-bottom: 1px solid #27272a;"">
                            <h1 style=""margin: 0; color: #f4f4f5; font-size: 24px; font-weight: 600;"">Verification Code</h1>
                        </td>
                    </tr>
                    
                    <!-- Main Content -->
                    <tr>
                        <td style=""padding: 40px;"">
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Hello,
                            </p>
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Your verification code is:
                            </p>
                            <div style=""background-color: #27272a; border-radius: 6px; padding: 20px; text-align: center; margin: 30px 0; border: 1px solid #3f3f46;"">
                                <span style=""font-size: 32px; font-weight: 600; letter-spacing: 8px; color: #f4f4f5;"">{otp}</span>
                            </div>
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Please do not share this code with anyone.
                            </p>
                            <p style=""margin: 0; color: #a1a1aa; font-size: 14px; line-height: 20px;"">
                                If you didn't request this code, please ignore this email.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 30px 40px; background-color: #27272a; border-top: 1px solid #3f3f46; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px;"">
                            <p style=""margin: 0; color: #a1a1aa; font-size: 14px; line-height: 20px; text-align: center;"">
                                This is an automated message, please do not reply.
                            </p>
                        </td>
                    </tr>
                </table>
                
                <!-- Company Info -->
                <table role=""presentation"" style=""max-width: 600px; width: 100%; border-collapse: collapse;"">
                    <tr>
                        <td style=""padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #71717a; font-size: 14px; line-height: 20px;"">
                                © 2024 NetFirm. All rights reserved.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage
            {
                From = new MailAddress(email!),
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true
            };

            message.To.Add(receptor);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            finally
            {
                message.Dispose();
                smtpClient.Dispose();
            }
        }

        public async Task SendEmailPassword(string receptor, string subject, string url)
        {
            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var bodyHtml = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Your OTP Code</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap');
    </style>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Inter', Arial, sans-serif; background-color: #09090b;"">
    <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #09090b; padding: 20px;"">
        <tr>
            <td align=""center"" style=""padding: 20px;"">
                <table role=""presentation"" style=""max-width: 600px; width: 100%; border-collapse: collapse; background-color: #18181b; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);"">
                    <!-- Header -->
                    <tr>
                        <td style=""padding: 30px 40px; text-align: center; border-bottom: 1px solid #27272a;"">
                            <h1 style=""margin: 0; color: #f4f4f5; font-size: 24px; font-weight: 600;"">Verification Code</h1>
                        </td>
                    </tr>
                    
                    <!-- Main Content -->
                    <tr>
                        <td style=""padding: 40px;"">
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Hello,
                            </p>
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Here is your reset password url:
                            </p>
                            <div style=""background-color: #18181b; border-radius: 6px; padding: 20px; text-align: center; margin: 30px 0; border: 1px solid #3f3f46;"">
    <a href=""{url}"" target=""_blank"" 
       style=""display: inline-block; font-size: 16px; font-weight: 600; letter-spacing: 1px; color: #f4f4f5; text-decoration: none; background-color: #2563eb; padding: 12px 24px; border-radius: 4px; border: none; transition: background-color 0.3s ease;"">
        Reset Password
    </a>
</div>
                            <p style=""margin: 0 0 20px; color: #e4e4e7; font-size: 16px; line-height: 24px;"">
                                Please do not share this url with anyone.
                            </p>
                            <p style=""margin: 0; color: #a1a1aa; font-size: 14px; line-height: 20px;"">
                                If you didn't request this url, please ignore this email.
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""padding: 30px 40px; background-color: #27272a; border-top: 1px solid #3f3f46; border-bottom-left-radius: 8px; border-bottom-right-radius: 8px;"">
                            <p style=""margin: 0; color: #a1a1aa; font-size: 14px; line-height: 20px; text-align: center;"">
                                This is an automated message, please do not reply.
                            </p>
                        </td>
                    </tr>
                </table>
                
                <!-- Company Info -->
                <table role=""presentation"" style=""max-width: 600px; width: 100%; border-collapse: collapse;"">
                    <tr>
                        <td style=""padding: 20px; text-align: center;"">
                            <p style=""margin: 0; color: #71717a; font-size: 14px; line-height: 20px;"">
                                © 2024 NetFirm. All rights reserved.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage
            {
                From = new MailAddress(email!),
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true
            };

            message.To.Add(receptor);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            finally
            {
                message.Dispose();
                smtpClient.Dispose();
            }
        }
    }
}
