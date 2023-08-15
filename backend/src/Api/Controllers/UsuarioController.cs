using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Models.ImageManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private IMediator _mediator;
    private IManageImageService _manageImageService;

    public UsuarioController(IMediator mediator, IManageImageService manageImageService)
    {
        _mediator = mediator;
        _manageImageService = manageImageService;
    }

    [AllowAnonymous]
    [HttpPost("login", Name = "login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand loginUser)
    {
        return await _mediator.Send(loginUser);
    }


    [AllowAnonymous]
    [HttpPost("register", Name = "register")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterUserCommand registerUser)
    {
        //VALIDAR LA IMAGEN
        if(registerUser.Foto is not null)
        {
            // SUBIR IMAGEN A CLOUDINARY
            var reusltImage = await _manageImageService.UploadImage(new ImageData
            {
                ImageStream = registerUser.Foto!.OpenReadStream(),
                Nombre = registerUser.Foto.Name
            });

            registerUser.FotoId = reusltImage.PublicId; 
            registerUser.FotoUrl = reusltImage.Url; 

        }

        return await _mediator.Send(registerUser);
    }
}
