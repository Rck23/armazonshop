using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Countries.Vms;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private IMediator _mediator;
        private readonly IAuthService _authService;

        public OrderController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpPost("address", Name ="CreateAddress")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddressVm>> CreateAddress([FromBody] CreateAddressCommand createAddress)
        {
            return await _mediator.Send(createAddress);
        }


    }
}
