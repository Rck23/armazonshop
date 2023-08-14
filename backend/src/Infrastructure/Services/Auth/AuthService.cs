using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Models.Token;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    //mapeo al token 
    public TokenSuperMaestro _tokensito { get; }
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(IOptions<TokenSuperMaestro> token, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _tokensito = token.Value;
    }
    public string CreateToken(Usuario usuario, IList<string>? roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),

            new Claim("userId", usuario.Id),

            new Claim("email", usuario.Email!),

        };

        foreach(var rol in roles!)
        {
            var claim = new Claim(ClaimTypes.Role, rol);
            claims.Add(claim);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokensito.key!));

        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescripcion = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_tokensito.ExpireTime),
            SigningCredentials = credenciales
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescripcion);

        return tokenHandler.WriteToken(token);

        
    }

    public string GetSessionUser()
    {
       var username = _contextAccessor.HttpContext!.User?
            .Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return username!;
    }
}
