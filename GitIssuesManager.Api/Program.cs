using GitIssuesManager.Api;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOptions<GitClientsConfiguration>()
  .Bind(builder.Configuration)
  .ValidateDataAnnotations()
  .ValidateOnStart();

builder.Services.RegisterGitHttpClients(builder.Configuration);

builder.Services.AddScoped<GithubIssuesClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/git/create", async (GithubIssuesClient client) =>
{
    await client.CreateIssue("misiek231", "git-issues-manager", new GithubUpdateIssueModel
    {
        Title = "Title",
        Body = "Body"
    });
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
