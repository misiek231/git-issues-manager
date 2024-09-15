using GitIssuesManager.Api;
using GitIssuesManager.Api.MinimalApiExtensions;
using GitIssuesManager.Api.Services;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<GitClientsConfiguration>()
  .Bind(builder.Configuration)
  .ValidateDataAnnotations()
  .Validate(p => p.Validate(), "Not all client types are configured in app settings")
  .ValidateOnStart();

builder.Services.RegisterGitHttpClients();
builder.Services.AddEndpointDefinitions();
builder.Services.AddScoped<GithubIssuesClient>();
builder.Services.AddScoped<GitlabIssuesClient>();
builder.Services.AddScoped<IssuesManagerService>();

var app = builder.Build();

app.UseEndpointDefinitions("api");
app.UseHttpsRedirection();

app.Run();