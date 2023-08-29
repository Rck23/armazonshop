using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Ecommerce.Application.Models.ImageManagement;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Models.Token;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.MessageImplementation;
using Ecommerce.Infrastructure.Persistence.Repositories;
using Ecommerce.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODOS LOS REPOSITORIOS & SERVICIOS
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        services.AddTransient<IEmailService, EmailService>();

        services.AddTransient<IAuthService, AuthService>();

        // PROPIEDADES DE TOKEN
        services.Configure<TokenSuperMaestro>(configuration.GetSection("TokenSuperMaestro"));
        // PROPIEDADES DE GUARDADO DE IMAGEN
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        // PROPIEDADES DEL EMAIL
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // PROPIEDADES DEL PAGOS
        services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

        return services;
    }
}
