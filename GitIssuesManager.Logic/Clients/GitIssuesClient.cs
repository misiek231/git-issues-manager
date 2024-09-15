using GitIssuesManager.Logic.Models;

namespace GitIssuesManager.Logic.Clients;

public abstract class GitIssuesClient(IHttpClientFactory httpClientFactory)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    protected abstract GitIssueClientType ClientType { get; }

    protected HttpClient GetClient() => _httpClientFactory.CreateClient(ClientType.ToString());
}
