using Microsoft.OpenApi.Models;

namespace API.Infrastructure.Configuration;

internal static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Rest API sample"
            });
            
            swagger.AddSecurityDefinition("JWT",new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "JWT",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "JWT"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
}