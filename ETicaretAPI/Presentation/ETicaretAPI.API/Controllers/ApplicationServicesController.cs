using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Features.Queries.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorizeDefinitionEndpoints()
        {
            List<GetAuthorizeDefinitionEndpointsQueryResponse> response = await _mediator.Send(new GetAuthorizeDefinitionEndpointsQueryRequest() { Assembly = typeof(Program).Assembly });
            return Ok(response);
        }
    }
}
