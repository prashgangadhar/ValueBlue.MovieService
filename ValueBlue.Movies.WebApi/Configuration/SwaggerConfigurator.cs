using Microsoft.OpenApi.Models;

namespace ValueBlue.Movies.WebApi.Configuration
{
    public static class SwaggerConfigurator
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Define the API Key security scheme
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "API Key Authentication for Admin Endpoints"
                });

                // Add custom operation filter to apply the security scheme only to admin endpoints
                options.OperationFilter<AdminApiKeyOperationFilter>();
            });
        }
    }
}
