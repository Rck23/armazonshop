using Ecommerce.Application.Features.Categories.Queries.GetCategoryList;
using Ecommerce.Application.Features.Countries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(typeof(IReadOnlyList<CountryVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<CountryVm>>> GetCategories()
        {
            var query = new GetCategoryListQuery();


            return Ok(await _mediator.Send(query));
        }
    }
}
