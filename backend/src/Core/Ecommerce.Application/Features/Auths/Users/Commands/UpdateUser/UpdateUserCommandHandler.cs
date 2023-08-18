﻿using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public UpdateUserCommandHandler(UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }


    public async Task<AuthResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // BUSCAR AL USUARIO A ACTUALIZAR
        var updateUsuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());

        if (updateUsuario is null)
        {
            throw new BadRequestException("El usuario no existe");
        }

        updateUsuario.Nombre = request.Nombre;
        updateUsuario.Apellido = request.Apellido;
        updateUsuario.Telefono = request.Telefono;
        updateUsuario.AvatarUrl = request.FotoUrl ?? updateUsuario.AvatarUrl;  //<-- SI NO SE QUIERE CAMBIAR LA FOTO O ES NULL

        // ACTUALIZAR LOS DATOS
        var resultado = await _userManager.UpdateAsync(updateUsuario);

        if(!resultado.Succeeded)
        {
            throw new BadRequestException("No se actualizo el usuario");
        }

        // OBTENER AL USUARIO YA ACTUALIZADO 
        var userById = await _userManager.FindByEmailAsync(request.Email!);
        var roles = await _userManager.GetRolesAsync(userById!);

        return new AuthResponse
        {
            Id = userById!.Id,
            Nombre = userById.Nombre,
            Apellido = userById.Apellido,
            Telefono = userById.Telefono,
            Email = userById.Email,
            Username = userById.UserName,
            Avatar = userById.AvatarUrl,
            Token = _authService.CreateToken(userById, roles),
            Roles = roles
        };
    
    }
}