using Microsoft.Extensions.Configuration;

namespace GitIssuesManager.Logic.Configuration;

public class GitClientConfig
{
    public string? Url {  get; set; }
    public string? AuthToken { get; set; }

    public static GitClientConfig? GetConfigFor(string providerName, IConfiguration configuration)
    {
        return configuration.GetSection("$GitClients:{providerName}").Get<GitClientConfig>();
    }
}
