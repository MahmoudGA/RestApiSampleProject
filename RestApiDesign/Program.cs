
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestApiDesign.Middleware;
using RestApiDesign.Models;
using RestApiDesign.Repositories;
using RestApiDesign.Services;
using RestApiDesign.Services.Interfaces;
using RestApiDesign.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Text;

namespace RestApiDesign
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var tracePath = Path.Join(path, $"Log_RestApiDesign_{DateTime.Now.ToString("yyyyMMdd")}.txt");
            Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(tracePath)));
            Trace.AutoFlush = true;

            var jwtConfig = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtConfig = builder.Configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"])),
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = jwtConfig["Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireNationalId", policy => policy.RequireClaim("nationalid"));
            });

            // Add services to the container.

            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"));
            });

            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( c=> {
                c.OperationFilter<AddNationalIdHeaderOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rest API Design Sample project", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var UseMockData = builder.Configuration.GetValue<bool>("UseMockData");

            if (UseMockData)
                builder.Services.AddTransient<ISinRepository, MockSinRepository>();
            else
                builder.Services.AddTransient<ISinRepository, CrmSinRepository>();

            builder.Services.AddTransient<ISinService, SinService>();
            builder.Services.AddScoped<JwtTokenService>();
            builder.Services.AddTransient<IApiResponseFactory, ApiResponseFactory>();
            
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorMessages = context.ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .SelectMany(ms => ms.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ApiResponse<string>
                    {
                        ErrorMessage = string.Join("; ", errorMessages),
                        ErrorCode = "ValidationError"
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<NationalIdValidationMiddleware>();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapFallback(() => Results.Redirect("/swagger"));
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
