using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly IAuthService _authService;
        readonly IConfiguration _configuration;

        public GoogleLoginCommandHandler(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.GoogleLoginAsync(new()
            {
                Id = request.Id,
                IdToken = request.IdToken,
                FirstName = request.FirstName,
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                PhotoUrl = request.PhotoUrl,
                Provider = request.Provider
            }, Convert.ToInt32(_configuration["JwtTokenLifeTimeSecond"]));

            return new() { Token = token };
        }
    }
}
