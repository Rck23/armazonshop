using FluentValidation;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{

    public CreateAddressCommandValidator()
    {
        RuleFor(p => p.Direccion).NotEmpty().WithMessage("La dirección no puede ser nula");
        RuleFor(p => p.Cuidad).NotEmpty().WithMessage("La cuidad no puede ser nula");
        RuleFor(p => p.Departamento).NotEmpty().WithMessage("El departamento no puede ser nulo");
        RuleFor(p => p.CodigoPostal).NotEmpty().WithMessage("El CP no puede ser nulo");
        RuleFor(p => p.Pais).NotEmpty().WithMessage("El país no puede ser nulo");
    }

}
