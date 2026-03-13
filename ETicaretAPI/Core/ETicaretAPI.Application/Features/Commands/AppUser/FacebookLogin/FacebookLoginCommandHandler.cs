using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IConfiguration _configuration;

        public FacebookLoginCommandHandler(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.FacebookLoginAsync(new()
            {
                Id = request.Id,
                AuthToken = request.AuthToken,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Name = request.Name,
                Email = request.Email,
                PhotoUrl = request.PhotoUrl,
                Provider = request.Provider
            }, Convert.ToInt32(_configuration["JwtTokenLifeTimeSecond"]));

            return new()
            {
                Token = token
            };
        }
    }
}
