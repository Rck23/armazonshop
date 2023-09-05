using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Orders;

public class OrderForCountingSpecification: BaseSpecification<Order>
{

    public OrderForCountingSpecification(OrderSpecificationParams specificationParams)
        : base(
            x => (string.IsNullOrEmpty(specificationParams.Username)
            || x.CompradorUsername!.Contains(specificationParams.Username)) &&
            (!specificationParams.Id.HasValue || x.Id == specificationParams.Id)
            )
    {
        
    }
}
