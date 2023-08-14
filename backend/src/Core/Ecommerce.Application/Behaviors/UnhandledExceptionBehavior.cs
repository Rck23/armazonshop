using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviours;

public class UnhandledExceptionBehavior<Trequest, Tresponse> : IPipelineBehavior<Trequest, Tresponse> where Trequest: IRequest<Tresponse>
{
    private readonly ILogger<Trequest> _logger;

    public UnhandledExceptionBehavior(ILogger<Trequest> logger)
    {
        _logger = logger;
    }


    public async Task<Tresponse> Handle(Trequest request, RequestHandlerDelegate<Tresponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(Trequest).Name;
           
            _logger.LogError(e, "Application Request: Sucedio una ecveption para el request {Name} {@Request}", request, requestName);

            throw new Exception("Application Request con Errores"); 
        }
    }
}
