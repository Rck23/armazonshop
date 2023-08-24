﻿using FluentValidation;

namespace Ecommerce.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator: AbstractValidator<CreateReviewCommand>
{

    public CreateReviewCommandValidator()
    {
        RuleFor(p=> p.Nombre)
            .NotNull()
            .WithMessage("Nombre no permite valores en blanco o nulos");

        RuleFor(p => p.Comentario)
          .NotNull()
          .WithMessage("Comentario no permite valores en blanco o nulos");

        RuleFor(p => p.Rating)
          .NotNull()
          .WithMessage("Rating no permite valores nulos");
    }

}
