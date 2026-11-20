using Microsoft.OpenApi;

namespace AiDocumentProcessing.Api;

public static class SwaggerConfigurator
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AI Document Assistant API",
                Version = "v1"
            });
        });

        return services;
    }

    public static WebApplication UseConfiguredSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("./v1/swagger.json", "AI Document Assistant API v1");
        });

        return app;
    }
}