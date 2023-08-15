using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{

    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("El correo electrónico no puede ser nulo");
        
        RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña no puede ser nula");
    
    
    }
}
