using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;
using System.Net.Http.Headers;

namespace GitIssuesManager.Api;

public static class StartupExtensions
{
    public static IServiceCollection RegisterGitHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var clients = Enum.GetValues<GitIssueClientType>();

        foreach (var client in clients)
        {
            var configResolver = GetConfigResolverFor(client);
            services.AddHttpClient(client.ToString(), p => configResolver(p, GitClientConfig.GetConfigFor(client, configuration)));
        }

        return services;
    }

    private static Action<HttpClient, GitClientConfig> GetConfigResolverFor(GitIssueClientType clientType) => clientType switch
    {
        GitIssueClientType.Github => ConfigureGithubClient,
        GitIssueClientType.Gitlab => throw new NotImplementedException()
    };

    private static void ConfigureGithubClient(HttpClient client, GitClientConfig clientConfig)
    {
        client.BaseAddress = new Uri(clientConfig.Url);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientConfig.AuthToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        client.DefaultRequestHeaders.Add("User-Agent", "git-issues-manager");
    }
}
