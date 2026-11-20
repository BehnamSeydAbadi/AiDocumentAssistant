using AiDocumentProcessing.Application.Common;

namespace AiDocumentProcessing.Application.Services;

public interface IAiApplicationService : IApplicationService
{
    Task<string> GetResponse(string prompt);
}