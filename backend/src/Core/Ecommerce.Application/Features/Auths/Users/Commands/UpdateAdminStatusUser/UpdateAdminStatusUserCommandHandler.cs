﻿using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;

public class UpdateAdminStatusUserCommandHandler : IRequestHandler<UpdateAdminStatusUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;

    public UpdateAdminStatusUserCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Usuario> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
    {
        var updateUsuario = await _userManager.FindByIdAsync(request.Id!);

        if (updateUsuario is null)
        {
            throw new BadRequestException("El usuario no existe!");
        }

        // SI ESTA ACTIVO TOMA DESACTIVO Y VICEVERSA
        updateUsuario.IsActive = !updateUsuario.IsActive;

        // ACTUALIZARLO
        var resultado = await _userManager.UpdateAsync(updateUsuario);

        if(!resultado.Succeeded)
        {
            throw new Exception("No se pudo actualizar el estado del usuario.");
        }

        return updateUsuario;
    }
}
