using ETicaretAPI.Application.Abstractions.Services.Configurations;
using MediatR;


namespace ETicaretAPI.Application.Features.Queries.Authorization
{
    public class GetAuthorizeDefinitionEndpointsQueryHandler : IRequestHandler<GetAuthorizeDefinitionEndpointsQueryRequest, List<GetAuthorizeDefinitionEndpointsQueryResponse>>
    {
        readonly IApplicationService _applicationService;

        public GetAuthorizeDefinitionEndpointsQueryHandler(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public Task<List<GetAuthorizeDefinitionEndpointsQueryResponse>> Handle(GetAuthorizeDefinitionEndpointsQueryRequest request, CancellationToken cancellationToken)
        {
           var menus = _applicationService.GetAuthorizeDefinitionEndpoints(request.Assembly);

            return Task.FromResult(menus.Select(m => new GetAuthorizeDefinitionEndpointsQueryResponse()
            {
                Name = m.Name,
                Actions = m.Actions
            }).ToList());

        }
    }
}
