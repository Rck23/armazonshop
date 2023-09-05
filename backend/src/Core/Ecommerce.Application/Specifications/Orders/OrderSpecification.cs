using Ecommerce.Domain;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Ecommerce.Application.Specifications.Orders;

public class OrderSpecification : BaseSpecification<Order>
{

    public OrderSpecification(OrderSpecificationParams specificationParams)
      : base(
          x => (string.IsNullOrEmpty(specificationParams.Username)
          || x.CompradorUsername!.Contains(specificationParams.Username)) &&
          (!specificationParams.Id.HasValue || x.Id == specificationParams.Id)
          )
    {

        AddInclude(p => p.OrderItems!);

        ApplyPaging(specificationParams.PageSize * (specificationParams.PageIndex - 1), specificationParams.PageSize);

        if (!string.IsNullOrEmpty(specificationParams.Sort))
        {
            switch(specificationParams.Sort)
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
            AddOrderByDescending(p => p.CreatedDate!);

        }
    }
}
