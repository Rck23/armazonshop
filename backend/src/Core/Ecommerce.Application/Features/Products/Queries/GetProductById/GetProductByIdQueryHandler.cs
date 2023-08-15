using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    //LA LOGICA DE LAS CONSULTAS 
    public async Task<ProductVm> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Product, Object>>>();
        includes.Add(p => p.Images!);
        includes.Add(p => p.Reviews!.OrderByDescending(x => x.CreatedDate));

        //CREAR LA CONSULTA
        var product = await _unitOfWork.Repository<Product>().GetEntityAsync(
            x => x.Id == request.ProductId,
            includes,
            true
        );

        return _mapper.Map<ProductVm>(product);
    }
}
