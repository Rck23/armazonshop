using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken;

public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByTokenQueryHandler(UserManager<Usuario> userManager, 
        IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userManager = userManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByNameAsync(_authService.GetSessionUser());

        if (usuario is null)
        {
            throw new Exception("El usuario no esta autenticado");
        }

        if(!usuario.IsActive)
        {
            throw new Exception("El usuario no esta activo, contacte al administrador");
        }

        var direccionEnvio = await _unitOfWork.Repository<Address>().GetEntityAsync(
                x => x.Username == usuario.UserName
        );

        var roles = await _userManager.GetRolesAsync(usuario);

        return new AuthResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            Username = usuario.UserName,
            Avatar = usuario.AvatarUrl,
            Roles = roles,
            DireccionEnvio = _mapper.Map<AddressVm>(direccionEnvio),
            Token = _authService.CreateToken(usuario, roles)
        };

    }
}
