using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Payments.Commands.CreatePayment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreatePayment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> CreatePayment([FromBody] CreatePaymentCommand createPayment)
        {
            return await _mediator.Send(createPayment);
        }
    }
}
