using Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetShoppingCartById;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {

        private IMediator _mediator;

        public ShoppingCartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> GetShoppingCart(Guid id)
        {
            var shoppingCartId = id == Guid.Empty ? Guid.NewGuid() : id;

            var query = new GetShoppingCartByIdQuery(shoppingCartId);

            return await _mediator.Send(query);
        }


        [AllowAnonymous]
        [HttpPut("{id}", Name = "UpdateShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartVm>> UpdateShoppingCart(Guid id, UpdateShoppingCartCommand updateShoppingCart)
        {
            updateShoppingCart.ShoppingCartId = id;


            return await _mediator.Send(updateShoppingCart);
        }

    }
}
