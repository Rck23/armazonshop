using Ecommerce.Application.Features.Reviews.Commands.CreateReview;
using Ecommerce.Application.Features.Reviews.Commands.DeleteReview;
using Ecommerce.Application.Features.Reviews.Queries.PaginationReviews;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name ="CreateReview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ReviewVm>> CreateReview([FromBody] CreateReviewCommand createReviewCommand)
        {
            return await _mediator.Send(createReviewCommand);
        }



        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("{id}", Name = "DeleteReview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> DeleteReview(int id)
        {
            var request = new DeleteReviewCommand(id); 
            return await _mediator.Send(request);
        }



        // PAGINACION DE REVIEWS 
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("paginationReviews", Name = "PaginationReview")]
        [ProducesResponseType(typeof(PaginationVm<ReviewVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Unit>> PaginationReview([FromQuery] PaginationReviewsQuery pagination)
        {
            var PaginationReview = await _mediator.Send(pagination);

            return Ok(PaginationReview);
            
        }

    }
}
