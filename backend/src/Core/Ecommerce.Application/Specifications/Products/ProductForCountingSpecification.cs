using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductForCountingSpecification : BaseSpecification<Product>
{
    public ProductForCountingSpecification(ProductSpecificationParams productSpecification) :
        base(
            x => (string.IsNullOrEmpty(productSpecification.Search) || x.Nombre!.Contains(productSpecification.Search) 
                    || x.Descripcion!.Contains(productSpecification.Search))
            && (!productSpecification.CategoryId.HasValue || x.CategoryId == productSpecification.CategoryId) 
            && (!productSpecification.PrecioMin.HasValue || x.Precio >= productSpecification.PrecioMin) 
            && (!productSpecification.PrecioMax.HasValue || x.Precio <= productSpecification.PrecioMax) 
            && (!productSpecification.Status.HasValue || x.Status == productSpecification.Status)
        )
    {

    }
}
