using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;
using Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;
using Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;
using Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserById;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken;
using Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUsername;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Application.Models.ImageManagement;
using Ecommerce.Domain;
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
    [HttpPost("Login", Name = "Login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand loginUser)
    {
        return await _mediator.Send(loginUser);
    }


    [AllowAnonymous]
    [HttpPost("Register", Name = "Register")]
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


    // CAMBIAR CONTRASEÑA 
    [AllowAnonymous]
    [HttpPost("ForgotPassword", Name = "ForgotPassword")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> ForgotPassword([FromBody] SendPasswordCommand sendPassword)
    {
        return await _mediator.Send(sendPassword);
    }

    [AllowAnonymous]
    [HttpPost("ResetPassword", Name = "ResetPassword")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordByTokenCommand resetPasswordByToken)
    {
        return await _mediator.Send(resetPasswordByToken);
    }


    // ACTUALIZAR CONTRASEÑA 
    [HttpPost("UpdatePassword", Name = "UpdatePassword")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<Unit>> UpdatePassword([FromBody] ResetPasswordCommand resetPassword)
    {
        return await _mediator.Send(resetPassword);
    }


    // ACTUALIZAR USUARIO
    [HttpPut("Update", Name = "Update")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Update([FromForm]  UpdateUserCommand updateUser)
    {
        // VERERIFICAR LA IMAGEN 
        if(updateUser.Foto is not null)
        {
             var resultImage = await _manageImageService.UploadImage(new ImageData
            {
                ImageStream = updateUser.Foto!.OpenReadStream(),
                Nombre = updateUser.Foto!.Name
            });

            updateUser.FotoId = resultImage.PublicId;
            updateUser.FotoUrl = resultImage.Url;
        }


        return await _mediator.Send(updateUser);
    }


    // ACTUALIZAR USUARIOS SIENDO ADMINISTRADOR 
    [Authorize(Roles =Role.ADMIN)]
    [HttpPut("UpdateAdminUser", Name = "UpdateAdminUser")]
    [ProducesResponseType(typeof(Usuario), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Usuario>> UpdateAdminUser([FromBody] UpdateAdminUserCommand updateUser)
    {
        return await _mediator.Send(updateUser);
    }


    // ACTUALIZAR EL ESTADO DEL USUARIO
    [Authorize(Roles = Role.ADMIN)]
    [HttpPut("UpdateAdminStatusUser", Name = "UpdateAdminStatusUser")]
    [ProducesResponseType(typeof(Usuario), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Usuario>> UpdateAdminStatusUser([FromBody] UpdateAdminStatusUserCommand updateAdminStatusUser)
    {
        return await _mediator.Send(updateAdminStatusUser);
    }


    // CONSULTA USUARIO POR ID
    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("{id}",  Name = "GetUsuarioById")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> GetUsuarioById(string id)
    {
        var query = new GetUserByIdQuery(id);


        return await _mediator.Send(query);
    }

    // CONSULTA USUARIO POR SESION TOKEN
    [HttpGet(Name = "CurrentUser")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> CurrentUser()
    {
        var query = new GetUserByTokenQuery();


        return await _mediator.Send(query);
    }



    // CONSULTA USUARIO POR USERNAME
    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("username/{username}", Name = "GetUsuarioByUsername")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> GetUsuarioByUsername(string username)
    {
        var query = new GetUserByUsernameQuery(username);


        return await _mediator.Send(query);
    }
}
