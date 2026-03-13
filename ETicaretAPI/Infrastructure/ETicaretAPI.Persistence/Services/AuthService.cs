using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.DTOs.Google;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
        }

        async Task<Token> CreateUserExternalAsync(AppUser user, UserLoginInfo userLoginInfo, string email, string name, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                //ilgili mail ile normal kullanıcı kaydı varsa onu al
                user = await _userManager.FindByEmailAsync(email);

                //o da yoksa ilgili kullanıcı dbde kayıtlı değil
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
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
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

                return token;
            }
            throw new Exception("Invalid external authentication");
        }

        public async Task<Token> FacebookLoginAsync(FacebookLoginRequest request, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["LoginServices:FacebookLogin:AppId"]}&client_secret={_configuration["LoginServices:FacebookLogin:AppSecretKey"]}&grant_type=client_credentials");

            //server'dan gönderilen appid, secretkey client'takiyle uyuşuyor mu response dönüyor
            FaceBookAccessTokenResponse? faceBookAccessTokenResponse = JsonSerializer.Deserialize<FaceBookAccessTokenResponse>(accessTokenResponse);

            //gelen response'taki token ile clienttan gelen token validasyon kontrolü 
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={faceBookAccessTokenResponse?.AccessToken}");

            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                UserLoginInfo userLoginInfo = new UserLoginInfo(request.Provider, validation.Data.UserId, request.Provider);

                Domain.Entities.Identity.AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                return await CreateUserExternalAsync(user, userLoginInfo, userInfo.Email, userInfo.Name, accessTokenLifeTime);
            }
            //todo exception
            throw new Exception("Invalid external authentication");
        }

        public async Task<Token> GoogleLoginAsync(GoogleLoginRequest request, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["LoginServices:GoogleLogin"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            UserLoginInfo userLoginInfo = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

            Domain.Entities.Identity.AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

            return await CreateUserExternalAsync(user, userLoginInfo, payload.Email, payload.Name, accessTokenLifeTime);
        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userNameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return token;
            }

            throw new AuthenticationErrorException();
        }
    }
}
