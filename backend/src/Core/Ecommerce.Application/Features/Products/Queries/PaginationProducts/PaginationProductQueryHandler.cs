using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Products;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts;

public class PaginationProductQueryHandler : IRequestHandler<PaginationProductQuery, PaginationVm<ProductVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaginationProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<ProductVm>> Handle(PaginationProductQuery request, CancellationToken cancellationToken)
    {
        var productSpecificationParams = new ProductSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Search = request.Search,
            Sort = request.Sort,
            CategoryId = request.CategoryId,
            PrecioMax = request.PrecioMax,
            PrecioMin = request.PrecioMin,
            Rating = request.Rating,
            Status = request.Status
        };

        // EN ProductSpecification SE ENCUENTRA TODO PARA HACER LA PAGINACION
        var spec = new ProductSpecification(productSpecificationParams);
        var products = await _unitOfWork.Repository<Product>().GetAllWithSpec(spec);


        var specCount = new ProductForCountingSpecification(productSpecificationParams);
        var totalProducts = await _unitOfWork.Repository<Product>().CountAsync(specCount);

        // CONVERTIR A VALOR DECIMAL REDONDEADO
        var rounded = Math.Ceiling(Convert.ToDecimal(totalProducts) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);


        // TRANSFORMAR products A COLECCION IReadOnlyList<ProductVm>
        var data = _mapper.Map<IReadOnlyList<ProductVm>>(products);

        var productsByPage = products.Count();

        var pagination = new PaginationVm<ProductVm>
        {
            Count = totalProducts,
            Data = data,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize= request.PageSize,
            ResultByPage = productsByPage
        };

        return pagination; 
    }
}
