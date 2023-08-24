﻿using Ecommerce.Application.Features.Reviews.Commands.CreateReview;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using MediatR;
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

    }
}
