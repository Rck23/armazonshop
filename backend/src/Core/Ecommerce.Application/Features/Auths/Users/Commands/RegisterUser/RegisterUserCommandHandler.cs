using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{

    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(UserManager<Usuario> userManager, 
        RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // VALIDACION DE USUARIO REGISTRADO
        var existeUserByEmail = await _userManager
            .FindByEmailAsync(request.Email!) is null ? false : true;

        if(existeUserByEmail)
        {
            throw new BadRequestException("El correo electrónico ya existe en la base de datos");
        }

        var existeUserByUsername = await _userManager
            .FindByNameAsync(request.Username!) is null ? false : true;

        if (existeUserByUsername)
        {
            throw new BadRequestException("El nombre del usuario ya existe en la base de datos");
        }

        //CREAR/REGISTRAR EL USUARIO
        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.Username,
            AvatarUrl = request.FotoUrl
        };

        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);

        if (resultado.Succeeded)
        {
            // SE AGREGA EL ROL
            await _userManager.AddToRoleAsync(usuario, AppRole.GenericUser);
        
            var roles = await _userManager.GetRolesAsync(usuario);

            // SE ENVIA LA DATA AL USUARIO
            return new AuthResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                Username = usuario.UserName,
                Avatar = usuario.AvatarUrl,
                Token = _authService.CreateToken(usuario, roles),
                Roles = roles
            }; 
        }

        throw new Exception("No se pudo registrar al usuario"); 
    
    }
}
