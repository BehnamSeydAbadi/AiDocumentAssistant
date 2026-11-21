using Microsoft.Extensions.Configuration;

namespace AiDocumentProcessing.Infrastructure.Test.Common;

internal class ConfigurationFixture
{
    private ConfigurationFixture()
    {
    }

    public static ConfigurationFixture New() => new();

    public AiConfiguration GetAiConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Test.json", optional: false)
            .Build();

        return config.GetSection(nameof(AiConfiguration)).Get<AiConfiguration>()!;
    }
}