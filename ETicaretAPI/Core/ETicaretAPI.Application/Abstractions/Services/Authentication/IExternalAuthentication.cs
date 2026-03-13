using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.DTOs.Google;

namespace ETicaretAPI.Application.Abstractions.Services.Authentication
{
    public interface IExternalAuthentication
    {
        Task<DTOs.Token> FacebookLoginAsync(FacebookLoginRequest request, int accessTokenLifeTime);
        Task<DTOs.Token> GoogleLoginAsync(GoogleLoginRequest request, int accessTokenLifeTime);
    }
}
