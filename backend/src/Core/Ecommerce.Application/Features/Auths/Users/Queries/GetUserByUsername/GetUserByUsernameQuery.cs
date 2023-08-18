using Ecommerce.Application.Features.Auths.Users.Vms;
using MediatR;


namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQuery: IRequest<AuthResponse>
{
    public string? UserName { get; set; }


    public GetUserByUsernameQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(userName);
    }



}
