using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;

public class UserForCountingSpecification: BaseSpecification<Usuario>
{
    public UserForCountingSpecification(UserSpecificationParams userSpecificatioinParams) : base(
            x =>
            (string.IsNullOrEmpty(userSpecificatioinParams.Search) ||
                x.Nombre!.Contains(userSpecificatioinParams.Search) ||
                x.Apellido!.Contains(userSpecificatioinParams.Search) ||
                x.Email!.Contains(userSpecificatioinParams.Search)
            )
    )
    {
        


    }

}
