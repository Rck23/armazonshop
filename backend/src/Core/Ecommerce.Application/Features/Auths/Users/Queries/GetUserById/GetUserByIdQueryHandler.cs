using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;

    public GetUserByIdQueryHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }


    public async Task<AuthResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByIdAsync(request.UserId!);

        if (usuario is null)
        {
            throw new BadRequestException("El usuario no existe!");
        }

        return new AuthResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            Username = usuario.UserName,
            Avatar = usuario.AvatarUrl,
            Roles = await _userManager.GetRolesAsync(usuario)
        };


    }
}
