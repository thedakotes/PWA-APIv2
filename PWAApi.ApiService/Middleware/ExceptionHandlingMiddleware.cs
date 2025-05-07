using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PWAApi.ApiService.Middleware
{
    
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Middleware exception caught!");

            var path = context.Request.Path;
            var isDev = false;

            //More log stuff        

            ProblemDetails response = exception switch
            {
                ApplicationException appEx => new ProblemDetails 
                { 
                    Status = StatusCodes.Status400BadRequest, 
                    Title = "Application exception occurred.",
                    Detail = isDev ? appEx.ToString() : appEx.Message,
                    Instance = path

                },
                ConfigurationException configEx => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Configuration error",
                    Detail = isDev ? configEx.ToString() : configEx.Message,
                    Instance = path
                },
                ValidationException valEx => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error",
                    Detail = isDev ? valEx.ToString() : valEx.Message,
                    Instance = path                   
                },
                UnauthorizedAccessException unAuth => new ProblemDetails
                { 
                    Status = StatusCodes.Status401Unauthorized, 
                    Title = "Unauthorized.",
                    Detail = isDev ? unAuth.ToString() : unAuth.Message,
                    Instance = path
                },
                _ => new ProblemDetails
                { 
                    Status = StatusCodes.Status500InternalServerError, 
                    Title = "Internal server error.",
                    Detail = isDev ? exception.ToString() : "An unexpected error occurred.",
                    Instance = path
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Status ?? 500;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
