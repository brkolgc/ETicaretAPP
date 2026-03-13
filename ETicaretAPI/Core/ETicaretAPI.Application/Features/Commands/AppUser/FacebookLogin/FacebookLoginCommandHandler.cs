using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;

        public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["LoginServices:FacebookLogin:AppId"]}&client_secret={_configuration["LoginServices:FacebookLogin:AppSecretKey"]}&grant_type=client_credentials");

            //server'dan gönderilen appid, secretkey client'takiyle uyuşuyor mu response dönüyor
            FaceBookAccessTokenResponse? faceBookAccessTokenResponse = JsonSerializer.Deserialize<FaceBookAccessTokenResponse>(accessTokenResponse);


            //gelen response'taki token ile clienttan gelen token validasyon kontrolü 
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={faceBookAccessTokenResponse.AccessToken}");

            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation.Data.IsValid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                UserLoginInfo userLoginInfo = new UserLoginInfo(request.Provider, validation.Data.UserId, request.Provider);

                Domain.Entities.Identity.AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                bool result = user != null;
                if (user == null)
                {
                    //ilgili mail ile normal kullanıcı kaydı varsa onu al
                    user = await _userManager.FindByEmailAsync(userInfo.Email);

                    //o da yoksa ilgili kullanıcı dbde kayıtlı değil
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo.Email,
                            UserName = userInfo.Email,
                            NameSurname = userInfo.Name
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
                {
                    await _userManager.AddLoginAsync(user, userLoginInfo); //aspNetUserLogins tablosuna kayıt at dış kaynaktan geldiği için

                    //token üret
                    Token token = _tokenHandler.CreateAccessToken(5);

                    return new()
                    {
                        Token = token
                    };
                }
            }

            throw new Exception("Invalid external authentication");
        }
    }
}
