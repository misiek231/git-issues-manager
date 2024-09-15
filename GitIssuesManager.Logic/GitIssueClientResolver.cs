using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GitIssuesManager.Logic;

public class GitIssueClientResolver(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public GitIssuesClient GetIssueClient(GitIssueClientType issueClientType) => issueClientType switch
    {
        GitIssueClientType.Github => serviceProvider.GetRequiredService<GithubIssuesClient>(),
        GitIssueClientType.Gitlab => serviceProvider.GetRequiredService<GitlabIssuesClient>(),
    };
}
