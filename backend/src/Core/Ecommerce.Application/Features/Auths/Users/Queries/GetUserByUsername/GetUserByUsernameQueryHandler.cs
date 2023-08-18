using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;

    public GetUserByUsernameQueryHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResponse> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByNameAsync(request.UserName!);

        if (usuario is null)
        {
            throw new Exception("El usuario no existe!");
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
