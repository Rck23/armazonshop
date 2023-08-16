using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;

public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
{

    private readonly UserManager<Usuario> _userManager;

    public ResetPasswordByTokenCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        // EVALUAR SI LA CONTRASEÑA Y LA CONFIRMACION DE CONTRASEÑA SON IGUALES
        if (!string.Equals(request.Password, request.ConfirmPassword))
        {
            throw new BadRequestException("La contraseña no es igual a la confirmación de contraseña");
        }

        // VALIDACION DE USUARIO
        var updateUser = await _userManager.FindByEmailAsync(request.Email!);
        if (updateUser is null)
        {
            throw new BadRequestException("El correo electrónico no esta registrado como usuario");
        }

        //VALIDACION DE TOKEN
        var token = Convert.FromBase64String(request.Token!);
        var tokenResult = Encoding.UTF8.GetString(token);

        // ACTIALIZAR LA CONTRASEÑA
        var cambioResultado = await _userManager.ResetPasswordAsync(updateUser, tokenResult, request.Password);

        // COMPARAMOS
        if (!cambioResultado.Succeeded)
        {
            throw new BadRequestException("No se pudo cambiar la contraseña");
        }

        return $"Se actualizo tu cuenta {request.Email} con tu nueva contraseña exitosamente";
    }
}
