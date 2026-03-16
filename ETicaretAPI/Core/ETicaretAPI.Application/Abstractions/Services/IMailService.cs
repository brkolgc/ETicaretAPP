using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMessageAsync(string to, string cc, string subject, string body, bool isBodyHtml = true);
        Task SendMessageAsync(string[] tos, string cc, string subject, string body, bool isBodyHtml = true);
    }
}
