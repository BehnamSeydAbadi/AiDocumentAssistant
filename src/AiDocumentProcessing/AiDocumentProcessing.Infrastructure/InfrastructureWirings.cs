using AiDocumentProcessing.Application.Services;
using AiDocumentProcessing.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiDocumentProcessing.Infrastructure;

public static class InfrastructureWirings
{
    public static void SetUpInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigAi(configuration);

        services.AddScoped<IAiApplicationService, AiApplicationService>();
    }
}