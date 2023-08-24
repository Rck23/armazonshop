using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Reviews;

public class ReviewForCountingSpecification: BaseSpecification<Review>
{
    public ReviewForCountingSpecification(ReviewSpecificationParams reviewSpecification) 
        :base(
            // EVALUAMOS LA EXISTENCIA
            X => 
            (!reviewSpecification.ProductId.HasValue || X.ProductId == reviewSpecification.ProductId))
    {
        
    }
}
