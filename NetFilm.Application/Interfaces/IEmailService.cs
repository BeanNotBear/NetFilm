using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
        Task SendEmailOtp(string receptor, string subject, string otp);
        Task SendEmailPassword(string receptor, string subject, string url);
    }
}
