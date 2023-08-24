using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Reviews;

public class ReviewSpecification: BaseSpecification<Review>
{

    public ReviewSpecification(ReviewSpecificationParams reviewSpecification)
    : base(
        // EVALUAMOS LA EXISTENCIA
        X =>
        (!reviewSpecification.ProductId.HasValue || X.ProductId == reviewSpecification.ProductId))
    {

        //DEVOLVER LOS RECORDS 
        ApplyPaging(reviewSpecification.PageSize * (reviewSpecification.PageIndex - 1), reviewSpecification.PageSize);


        // ORDENAMIENTO
        if (!string.IsNullOrEmpty(reviewSpecification.Sort))
        {
            switch (reviewSpecification.Sort)
            {
                case "createDateAsc":
                    AddOrderBy(p => p.CreatedDate!);
                    break;

                case "createDateDesc":
                    AddOrderByDescending(p => p.CreatedDate!);
                    break;

                default:
                    AddOrderBy(p => p.CreatedDate!);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(P => P.CreatedDate!);
        }
    }
}
