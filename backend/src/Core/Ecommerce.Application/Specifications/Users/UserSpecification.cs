using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;

public class UserSpecification: BaseSpecification<Usuario>
{

    public UserSpecification(UserSpecificationParams userSpecificatioinParams):base(
            x =>
            (string.IsNullOrEmpty(userSpecificatioinParams.Search) ||
                x.Nombre!.Contains(userSpecificatioinParams.Search) || 
                x.Apellido!.Contains(userSpecificatioinParams.Search) || 
                x.Email!.Contains(userSpecificatioinParams.Search)
            )
    )
    {

        ApplyPaging(userSpecificatioinParams.PageSize * (userSpecificatioinParams.PageIndex - 1), userSpecificatioinParams.PageSize);

        if (!string.IsNullOrEmpty(userSpecificatioinParams.Sort))
        {
            switch (userSpecificatioinParams.Sort)
            {
                case "nombreAsc":
                    AddOrderBy(p => p.Nombre); 
                    break;

                case "nombreDesc":
                    AddOrderByDescending(p => p.Nombre);
                    break;

                case "apellidoAsc":
                    AddOrderBy(p => p.Apellido);
                    break;

                case "apellidoDesc":
                    AddOrderByDescending(p => p.Apellido);
                    break;

                default:
                    AddOrderBy(p => p.Nombre);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(p => p.Nombre);
        }
    }
}
