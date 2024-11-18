using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestApiDesign.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RestApiDesign.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IApiResponseFactory _apiResponseFactory;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IApiResponseFactory apiResponseFactory)
        {
            _next = next;
            _logger = logger;
            _apiResponseFactory = apiResponseFactory;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument Exception: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (ValidationException ex)
            {
                var response = _apiResponseFactory.CreateErrorResponse(ex.Message);
                
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 400;
                _logger.LogError(ex, "Validation Exception: {Message}", ex.Message);
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                
                //await HandleExceptionAsync(httpContext, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, "An unexpected error occurred.", StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string errorMessage, int statusCode)
        {
            var response = _apiResponseFactory.CreateErrorResponse(errorMessage);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }


}
