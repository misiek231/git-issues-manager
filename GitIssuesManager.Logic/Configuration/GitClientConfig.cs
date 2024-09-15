using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.Configuration;

namespace GitIssuesManager.Logic.Configuration;

public class GitClientConfig
{
    public required string Url { get; set; }
    public required string AuthToken { get; set; }

    public static GitClientConfig GetConfigFor(GitIssueClientType clientType, IConfiguration configuration)
    {
        return configuration.GetSection($"GitClients:{clientType}").Get<GitClientConfig>() ?? throw new Exception($"Missing or invalid confoguration for {clientType} client");
    }
}
