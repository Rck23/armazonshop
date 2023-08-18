using Ecommerce.Application.Features.Auths.Users.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserById;

public class GetUserByIdQuery: IRequest<AuthResponse>
{
    public string? UserId { get; set; }

    public GetUserByIdQuery(string userId)
    {
        //VALIDAR ID SEA REAL
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
    }

}
