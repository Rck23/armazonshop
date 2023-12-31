﻿using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(UserManager<Usuario> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }


    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {

        // VALIDAR USUARIO
        var updateUsuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());

        if (updateUsuario is null)
        {
            throw new BadRequestException("El usuario no existe");
        }

        // VALIDAR LA CONTRASEÑA ANTIGUA
        var resultValidateOldPassword = _userManager.PasswordHasher
            .VerifyHashedPassword(updateUsuario, updateUsuario.PasswordHash!, request.OldPassword!);

        if(!(resultValidateOldPassword == PasswordVerificationResult.Success))
        {
            throw new BadRequestException("La contraseña ingresada es erronea");
        }

        // ENCRIPTAR LA NUEVA CONTRASEÑA
        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUsuario, request.NewPassword!);
        updateUsuario.PasswordHash = hashedNewPassword;

        //ACTUALIZAR EL USUARIO
        var resultado = await _userManager.UpdateAsync(updateUsuario);

        if (!resultado.Succeeded)
        {
            throw new BadRequestException("¡Algo salio mal! No se pudo cambiar la contraseña");
        }

        return Unit.Value;
    }
}
