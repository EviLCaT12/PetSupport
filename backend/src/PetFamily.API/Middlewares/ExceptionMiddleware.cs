using System.Net;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            var responseError = Error.Conflict("server.invalid", e.Message);
            
            var envelope = Envelope.Error(new ErrorList([responseError])); 
            
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await httpContext.Response.WriteAsJsonAsync(envelope);
        }
        
    }
}