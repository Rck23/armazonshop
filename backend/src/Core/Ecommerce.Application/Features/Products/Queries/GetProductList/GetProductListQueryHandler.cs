﻿using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<Product>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetProductListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IReadOnlyList<Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        // REFERENCIAS A LA ENTIDADES DE PRODUCTOS
        var includes = new List<Expression<Func<Product, object>>>();

        includes.Add(p => p.Images!);
        includes.Add(p => p.Reviews!);

        var productos = await _unitOfWork.Repository<Product>().GetAsync(
                null,
                x => x.OrderBy(y => y.Nombre),
                includes,
                true
            ) ;

        return productos; 
    }
}
