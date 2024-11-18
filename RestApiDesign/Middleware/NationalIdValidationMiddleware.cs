using RestApiDesign.Services.Interfaces;

namespace RestApiDesign.Middleware
{
    public class NationalIdValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISinService _sinService;

        public NationalIdValidationMiddleware(RequestDelegate next, ISinService sinService)
        {
            _next = next;
            _sinService = sinService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
         
            if (path.Contains("/swagger") || path.Contains("/api/auth/login"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("NationalId", out var nationalId) || string.IsNullOrWhiteSpace(nationalId))
            {
                await ReturnUnauthorizedResponse(context, "Unauthorized: Missing or invalid NationalId header.");
                return;
            }

            if (!_sinService.NationalIdExists(nationalId).Result)
            {
                await ReturnUnauthorizedResponse(context, "Unauthorized: Missing or invalid NationalId header.");
                return;
            }

            await _next(context);
        }

        private async Task ReturnUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync("Unauthorized: Missing or invalid NationalId header."); 
        }
    }

}
