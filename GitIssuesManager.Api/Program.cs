using GitIssuesManager.Api;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOptions<GitClientsConfiguration>()
  .Bind(builder.Configuration)
  .ValidateDataAnnotations()
  .Validate(p => p.Validate(), "Not all client types are configured in app settings")
  .ValidateOnStart();

builder.Services.RegisterGitHttpClients(builder.Configuration);

builder.Services.AddScoped<GithubIssuesClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/git/create", async (GithubIssuesClient client) =>
{
    await client.CreateIssue("misiek231", "git-issues-manager", new GithubUpdateIssueModel
    {
        Title = "Title",
        Body = "Body"
    });
});

app.Run();