using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestApiDesign.Swagger
{
    public class AddNationalIdHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "NationalId",
                In = ParameterLocation.Header,
                Required = true, 
                Description = "NationalId header",
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
