using Ecommerce.Application.Models.Token;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODOS LOS REPOSITORIOS
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        // PROPIEDADES DE TOKEN
        services.Configure<TokenSuperMaestro>(configuration.GetSection("TokenSuperMaestro"));

        return services;
    }
}
