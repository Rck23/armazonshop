using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand> 
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty()
          .WithMessage("El nombre no puede estar en blanco")
          .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres");

        RuleFor(x => x.Descripcion).NotEmpty()
            .WithMessage("La descripción no puede ser nula");

            RuleFor(x => x.Stock).NotEmpty()
        .WithMessage("La stock no puede ser nulo");

            RuleFor(x => x.Precio).NotEmpty()
        .WithMessage("El precio no puede ser nulo");
    }
}
