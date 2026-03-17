namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string cc, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string cc, string subject, string body, bool isBodyHtml = true);

        Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
    }
}
