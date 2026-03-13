using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken
{
    public class RefreshTokenLoginCommandRequest : IRequest<RefreshTokenLoginCommandResponse>
    {
        public string RefreshToken { get; set; }
    }
}
