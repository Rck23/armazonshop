using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Orders.Commands.CreateOrder;
using Ecommerce.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Application.Features.Orders.Queries.GetOrdersById;
using Ecommerce.Application.Features.Orders.Queries.PaginationOrders;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost(Name = "CreateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> CreateOrder([FromBody] CreateOrderCommand createOrder)
        {
            return await _mediator.Send(createOrder);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> UpdateOrder([FromBody] UpdateOrderCommand updateOrder )
        {
            return await _mediator.Send(updateOrder);
        }



        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(typeof(OrderVm) ,(int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderVm>> GetOrderById(int id)
        {

            var query = new GetOrdersByIdQuery(id);

            return Ok(await _mediator.Send(query)); 
        }


        // PAGINACION SOBRE UN DETERMINADO USUARIO EN SESION 
        [HttpGet("paginationByUsername", Name = "PaginationOrderByUsername")]
        [ProducesResponseType(typeof(PaginationVm<OrderVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<OrderVm>>> PaginationOrderByUsername([FromQuery] PaginationOrdersQuery pagination)
        {
            // OBETENER DEL TOKEN EL USUARIO QUE ESTA EN SESION
            pagination.Username = _authService.GetSessionUser();

            var paginationData = await _mediator.Send(pagination);

            return Ok(paginationData);

        }


        // PAGINACION CUANDO EL USUARIO SEA ADMINISTRADOR 
        [Authorize(Roles =Role.ADMIN)]
        [HttpGet("PaginationAdmin", Name = "PaginationOrder")]
        [ProducesResponseType(typeof(PaginationVm<OrderVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<OrderVm>>> PaginationOrder([FromQuery] PaginationOrdersQuery pagination)
        {
            // OBETENER DEL TOKEN EL USUARIO QUE SEA ADMINISTRADOR

            var paginationData = await _mediator.Send(pagination);

            return Ok(paginationData);

        }
    }
}
