using Ecommerce.Domain;

namespace Ecommerce.Application.Contracts.Identity;

public interface IAuthService
{
    // obtener la sesion del usuario
    string GetSessionUser();

    //crear el token basado en las propiedades del usuario en sesion
    string CreateToken(Usuario usuario, IList<string>? roles); 

}
