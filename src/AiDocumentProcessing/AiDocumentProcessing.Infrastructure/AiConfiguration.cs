using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiDocumentProcessing.Infrastructure;

public class AiConfiguration
{
    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
}

public static class AiConfigurator
{
    public static void ConfigAi(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(nameof(AiConfiguration));
        var aiConfiguration = configurationSection.Get<AiConfiguration>()!;
        services.AddSingleton(aiConfiguration);
    }
}