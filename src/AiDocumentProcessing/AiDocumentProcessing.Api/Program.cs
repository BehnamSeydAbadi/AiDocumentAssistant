using AiDocumentProcessing.Api;
using AiDocumentProcessing.Api.Endpoints;
using AiDocumentProcessing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

builder.Services.SetUpInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

SampleEndpoints.Map(app);

app.UseConfiguredSwagger();

app.Run();