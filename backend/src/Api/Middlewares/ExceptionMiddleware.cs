using Ecommerce.Api.Errors;
using Ecommerce.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Ecommerce.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next; 
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger; 
        _next = next;
    }


    // EVALUA EL REQUEST DELEGATE
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception e)
        {
            // SI HAY ERROR 
            _logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";

            // VALOR POR DEFECTO
            var statusCode = (int)HttpStatusCode.InternalServerError; 
            // RESULTADO POR EFECTO
            var result = string.Empty;

            switch (e)
            {
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break; 

                case FluentValidation.ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var errors = validationException.Errors.Select(ers => ers.ErrorMessage).ToArray();
                    var validationJsons = JsonConvert.SerializeObject(errors);
                    result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, errors, validationJsons));
                    break; 

                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;


                // SI NO SON LOS CASOS DE ARRIBA
                default: statusCode = (int)HttpStatusCode.InternalServerError; break;

            }

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new 
                    CodeErrorException(statusCode, new string[]{ e.Message, e.StackTrace}));
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
