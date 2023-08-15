using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams productSpecification)
        :base(
            x => (string.IsNullOrEmpty(productSpecification.Search) || x.Nombre!.Contains(productSpecification.Search)
                    || x.Descripcion!.Contains(productSpecification.Search))
            && (!productSpecification.CategoryId.HasValue || x.CategoryId == productSpecification.CategoryId)
            && (!productSpecification.PrecioMin.HasValue || x.Precio >= productSpecification.PrecioMin)
            && (!productSpecification.PrecioMax.HasValue || x.Precio <= productSpecification.PrecioMax)
            && (!productSpecification.Status.HasValue || x.Status == productSpecification.Status)

         )
    {
        AddInclude(p => p.Reviews!);
        AddInclude(p => p.Images!);

        // APLICAR LA PAGINACION
        ApplyPaging(productSpecification.PageSize * (productSpecification.PageIndex - 1), productSpecification.PageSize);

        // ORDNENAMIENOT
        if (!string.IsNullOrEmpty(productSpecification.Sort))
        {
            switch (productSpecification.Sort)
            {
                case "nombreAsc":
                    AddOrderBy(p => p.Nombre!);
                    break;

                case "nombreDesc":
                    AddOrderByDescending(p => p.Nombre!);
                    break;

                case "precioAsc":
                    AddOrderBy(p => p.Precio!);
                    break;

                case "precioDesc":
                    AddOrderByDescending(p => p.Precio!);
                    break;

                case "ratingAsc":
                    AddOrderBy(p => p.Rating!);
                    break;

                case "ratingDesc":
                    AddOrderByDescending(p => p.Rating!);
                    break;

                default:
                    AddOrderBy(p => p.CreatedDate!);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(p => p.CreatedDate!);
        }
    }
}
