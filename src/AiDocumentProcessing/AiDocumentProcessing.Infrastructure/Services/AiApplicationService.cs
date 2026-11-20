using System.ClientModel;
using AiDocumentProcessing.Application.Services;
using OpenAI;
using OpenAI.Chat;

namespace AiDocumentProcessing.Infrastructure.Services;

public class AiApplicationService(AiConfiguration aiConfiguration) : IAiApplicationService
{
    public async Task<string> GetResponse(string prompt)
    {
        var options = new OpenAIClientOptions { Endpoint = new Uri(aiConfiguration.BaseUrl) };
        var chatClient = new ChatClient(
            model: "o4-mini",
            credential: new ApiKeyCredential(aiConfiguration.ApiKey),
            options: options
        );

        var message = new UserChatMessage(prompt);
        var chatResult = await chatClient.CompleteChatAsync([message]);
        return chatResult.Value.Content.First().Text!;
    }
}