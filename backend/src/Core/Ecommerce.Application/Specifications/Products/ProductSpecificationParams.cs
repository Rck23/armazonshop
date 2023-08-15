using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductSpecificationParams: SpecificationParams
{
    // TODAS LAS PROPIEDADES PUEDEN SER NULAS
    public int? CategoryId { get; set; }
    public decimal? PrecioMax { get; set; }
    public decimal? PrecioMin { get; set; }
    public decimal? Rating { get; set; }
    public ProductStatus? Status { get; set; }
}
