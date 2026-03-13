using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly IConfiguration _configuration;
        public UserService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                NameSurname = model.NameSurname
            }, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturuldu.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}";

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenExpiredDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiredDate = accessTokenExpiredDate.AddSeconds(Convert.ToInt32(_configuration["JwtRefreshTokenLifeTimeSecond"]));
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }
    }
}
