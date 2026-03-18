using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenExpiredDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<(List<ListUser>,int TotalUsersCount)> GetAllUsersAsync(int page, int size);
        Task AssignRoleToUserAsync(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userId);
    }
}
