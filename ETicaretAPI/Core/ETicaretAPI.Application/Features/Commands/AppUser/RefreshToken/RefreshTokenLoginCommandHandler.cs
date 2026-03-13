using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        readonly IAuthService _authService;

        public RefreshTokenLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
            return new()
            {
                Token = token
            };
        }
    }
}
