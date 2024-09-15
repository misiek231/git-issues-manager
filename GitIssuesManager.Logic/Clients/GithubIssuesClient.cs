using GitIssuesManager.Logic.Models;
using OneOf;
using OneOf.Types;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Clients;

public class GithubIssuesClient(IHttpClientFactory clientFactory)
{
    private readonly IHttpClientFactory clientFactory = clientFactory;

    public async Task<OneOf<ResultModel, Error>> CreateIssue(string owner, string repo, CreateIssueModel model)
    {
        //var client = SetupClient();

        var client = clientFactory.CreateClient("GithubClient");

        var response = await client.PostAsJsonAsync($"/repos/{owner}/{repo}/issues", model);

        if (response.IsSuccessStatusCode)
        {
            // TODO: return proper error
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

    public async Task<OneOf<ResultModel, Error>> UpdateIssue(string owner, string repo, string issueNumber, UpdateIssueModel model)
    {
        var client = clientFactory.CreateClient("GithubClient");

        var response = await client.PostAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", model);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

    record CloseIssueModel(string State = "closed");

    public async Task<OneOf<ResultModel, Error>> CloseIssue(string owner, string repo, string issueNumber)
    {
        var client = clientFactory.CreateClient("GithubClient");

        var response = await client.PostAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", new CloseIssueModel());

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

/*    private HttpClient SetupClient()
    {
        ArgumentException.ThrowIfNullOrEmpty(clientConfig.Url);
        ArgumentException.ThrowIfNullOrEmpty(clientConfig.AuthToken);

        var client = new HttpClient
        {
            BaseAddress = new Uri(clientConfig.Url),
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientConfig.AuthToken);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        client.DefaultRequestHeaders.Add("User-Agent", "git-issues-manager");

        return client;
    }*/
}
