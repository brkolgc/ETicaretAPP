using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.CustomAttribues;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Queries.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Authorize Definition Endpoints", Menu = "Application Services")]
        public async Task<IActionResult> GetAuthorizeDefinitionEndpoints()
        {
            List<GetAuthorizeDefinitionEndpointsQueryResponse> response = await _mediator.Send(new GetAuthorizeDefinitionEndpointsQueryRequest() { Assembly = typeof(Program).Assembly });
            return Ok(response);
        }
    }
}
