using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly IConfiguration _configuration;
        readonly ITokenHandler _tokenHandler;
        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, IConfiguration configuration, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["LoginServices:GoogleLogin"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            UserLoginInfo userInfo = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

            Domain.Entities.Identity.AppUser? user = await _userManager.FindByLoginAsync(userInfo.LoginProvider, userInfo.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                //ilgili mail ile normal kullanıcı kaydı varsa onu al
                user = await _userManager.FindByEmailAsync(payload.Email);

                //o da yoksa ilgili kullanıcı dbde kayıtlı değil
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };

                    var identityResult = await _userManager.CreateAsync(user); //aspNetUser tablosuna kayıt at
                    result = identityResult.Succeeded;
                }
                else
                {
                    //dbde ilgili mail ile kullanıcı kaydı var result=true
                    result = true;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, userInfo); //aspNetUserLogins tablosuna kayıt at dış kaynaktan geldiği için
            else
                throw new Exception("Invalid external authentication");

            //token üret
            Token token = _tokenHandler.CreateAccessToken(5);

            return new()
            {
                Token = token
            };
        }
    }
}
