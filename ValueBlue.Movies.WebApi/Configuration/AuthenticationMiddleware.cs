namespace ValueBlue.Movies.WebApi.Configuration
{
    public static class AuthenticationMiddleware
    {
        public static void ConfigureApiKeyAuthenticationForAdminEndpoints(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/Admin"))
                {
                    var config = context.RequestServices.GetRequiredService<IConfiguration>();
                    var apiKey = config["AdminApiKey"];

                    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("API Key was not provided. Client Unauthorized.");
                        return;
                    }
                    if(extractedApiKey != apiKey)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("API key provided is invalid. Client Forbidden.");
                        return;
                    }
                }

                await next();
            });
        }
    }
}
