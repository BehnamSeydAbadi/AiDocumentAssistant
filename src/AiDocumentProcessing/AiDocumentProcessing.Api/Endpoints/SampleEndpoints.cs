using AiDocumentProcessing.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiDocumentProcessing.Api.Endpoints;

public class SampleEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/sample",
            async ([FromServices] IAiApplicationService aiApplicationService, [FromQuery] string prompt) =>
            {
                var response = await aiApplicationService.GetResponse(prompt);
                return Results.Ok(response);
            });
    }
}