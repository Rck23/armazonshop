using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<ProductVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<IReadOnlyList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
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

        var productVm = _mapper.Map<IReadOnlyList<ProductVm>>( productos );

        return productVm; 
    }
}
