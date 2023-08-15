using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts;

public class PaginationProductQuery: PaginationBaseQuery, IRequest<PaginationVm<ProductVm>>
{
    public int? CategoryId { get; set; }
    public int? Rating { get; set; }
    public decimal? PrecioMax { get; set; }
    public decimal? PrecioMin { get; set; }

    public ProductStatus? Status { get; set; }


}
