using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string cc, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, cc, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string cc, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);

            if (!string.IsNullOrWhiteSpace(cc))
                mail.CC.Add(cc);

            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:From"], _configuration["Mail:DisplayName"], System.Text.Encoding.UTF8);

            SmtpClient smtp = new(_configuration["Mail:Host"]);
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = Convert.ToInt32(_configuration["Mail:Port"]);
            smtp.EnableSsl = Convert.ToBoolean(_configuration["Mail:EnableSsl"]);

            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            var resetUrl = $"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}";

            var mailBody = $@"
                            <p>Merhaba,</p>
                            <p>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz:</p>
                            <p><strong><a target=""_blank"" href=""{resetUrl}"">Yeni şifre talebi için tıklayınız...</a></strong></p>
                            <p style=""font-size:12px;"">
                            NOT: Bu şifre yenileme talebini siz yapmadıysanız, lütfen bu e-postayı dikkate almayınız.
                            </p>
                            <br>
                            <p><strong>E-Ticaret Demo</strong></p>
                            ";

            await SendMailAsync(to, "", "Şifre Yenileme Talebi", mailBody);
        }
    }
}
