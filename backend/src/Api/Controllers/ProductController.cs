﻿using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Application.Models.ImageManagement;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        // CLASE DEL MEDIADOR 
        private IMediator _mediator;

        private IManageImageService _manageImageService;

        public ProductController(IMediator mediator, IManageImageService manageImage)
        {
            _mediator = mediator;
            _manageImageService = manageImage;
        }

        [AllowAnonymous]
        [HttpGet("list", Name = "GetProductList")]
        [ProducesResponseType(typeof(IReadOnlyList<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IReadOnlyList<ProductVm>>> GetProductList()
        {
            var query = new GetProductListQuery();

            var productos = await _mediator.Send(query);

            return Ok(productos);
        }

        [AllowAnonymous]
        [HttpGet("pagination", Name = "PaginationProduct")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationProduct(
            [FromQuery] PaginationProductQuery paginationProductsQuery
        )
        {
            paginationProductsQuery.Status = ProductStatus.Activo;
            var paginationProduct = await _mediator.Send(paginationProductsQuery);
            return Ok(paginationProduct);
        }

        // PAGINACION DE PRODUCTOS SOLO ADMINISTRADORES
        [Authorize(Roles = Role.ADMIN)]
        [HttpGet("paginationAdmin", Name = "PaginationProductAdmin")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationAdmin(
                [FromQuery] PaginationProductQuery paginationProductsQuery
        )
        {
            var paginationProduct = await _mediator.Send(paginationProductsQuery);
            return Ok(paginationProduct);
        }


        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> GetProductById(int id)
        {

            var query = new GetProductByIdQuery(id);
            return Ok(await _mediator.Send(query));

        }


        // CREAR PRODUCTOS
        [Authorize(Roles = Role.ADMIN)]
        [HttpPost("create", Name = "CreateProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> CreateProduct([FromForm] CreateProductCommand createProduct)
        {
            var listFotosUrls = new List<CreateProductImageCommand>();

            // CARGAR LAS IMAGENES
            if (createProduct.Fotos is not null)
            {
                foreach (var foto in createProduct.Fotos)
                {
                    var resultImage = await _manageImageService.UploadImage(new Application.Models.ImageManagement.ImageData
                    {
                        ImageStream = foto.OpenReadStream(),
                        Nombre = foto.Name
                    });

                    var fotoCommand = new CreateProductImageCommand
                    {
                        PublicCode = resultImage.PublicId,
                        Url = resultImage.Url
                    };

                    listFotosUrls.Add(fotoCommand);
                }

                createProduct.ImageUrls = listFotosUrls;
            }

            // ENVIAR AL HANDLER
            return await _mediator.Send(createProduct);
        }


        [Authorize(Roles = Role.ADMIN)]
        [HttpPut("update", Name = "UpdateProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> UpdateProduct([FromForm] UpdateProductCommand request)
        {
            var listFotoUrls = new List<CreateProductImageCommand>();

            if (request.Fotos is not null)
            {
                foreach (var foto in request.Fotos)
                {
                    var resultImage = await _manageImageService.UploadImage(new ImageData
                    {
                        ImageStream = foto.OpenReadStream(),
                        Nombre = foto.Name
                    });

                    var fotoCommand = new CreateProductImageCommand
                    {
                        PublicCode = resultImage.PublicId,
                        Url = resultImage.Url
                    };

                    listFotoUrls.Add(fotoCommand);
                }
                request.ImageUrls = listFotoUrls;
            }

            return await _mediator.Send(request);

        }




        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("status/{id}", Name = "UpdateStatusProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductVm>> UpdateStatusProduct(int id)
        {
            var request = new DeleteProductCommand(id);
            return await _mediator.Send(request);
        }




    }
}
