using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace GitIssuesManager.Api;

public static class StartupExtensions
{
    public static IServiceCollection RegisterGitHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var clientTypes = Enum.GetValues<GitIssueClientType>();

        foreach (var clientType in clientTypes)
        {
            services.AddHttpClient(clientType.ToString(), GetConfigResolverFor(clientType));
        }

        return services;
    }

    private static Action<IServiceProvider, HttpClient> GetConfigResolverFor(GitIssueClientType clientType) => clientType switch
    {
        GitIssueClientType.Github => ConfigureGithubClient,
        GitIssueClientType.Gitlab => ConfigureGitlabClient,
    };

    private static void ConfigureGithubClient(IServiceProvider services, HttpClient client)
    {
        var clientConfig = services.GetRequiredService<IOptions<GitClientsConfiguration>>()
            .Value
            .GitClients[GitIssueClientType.Github];
        
        client.BaseAddress = new Uri(clientConfig.Url);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientConfig.AuthToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        client.DefaultRequestHeaders.Add("User-Agent", "git-issues-manager");
    }

    private static void ConfigureGitlabClient(IServiceProvider services, HttpClient client)
    {
        var clientConfig = services.GetRequiredService<IOptions<GitClientsConfiguration>>()
            .Value
            .GitClients[GitIssueClientType.Gitlab];

        client.BaseAddress = new Uri(clientConfig.Url);

        client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", clientConfig.AuthToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "git-issues-manager");
    }
}
