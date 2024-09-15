using GitIssuesManager.Logic.Models;
using OneOf;
using OneOf.Types;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Clients;

public class GithubIssuesClient(IHttpClientFactory clientFactory) : GitIssuesClient(clientFactory)
{
    protected override GitIssueClientType ClientType => GitIssueClientType.Github;

    public async Task<OneOf<ResultModel, Error>> CreateIssue(string owner, string repo, GithubUpdateIssueModel model)
    {
        var client = GetClient();

        var response = await client.PostAsJsonAsync($"/repos/{owner}/{repo}/issues", model);

        if (response.IsSuccessStatusCode)
        {
            // TODO: return proper error
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

    public async Task<OneOf<ResultModel, Error>> UpdateIssue(string owner, string repo, string issueNumber, GithubUpdateIssueModel model)
    {
        var client = GetClient();

        var response = await client.PatchAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", model);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

    record CloseIssueModel(string State = "closed");

    public async Task<OneOf<ResultModel, Error>> CloseIssue(string owner, string repo, string issueNumber)
    {
        var client = GetClient();

        var response = await client.PatchAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", new CloseIssueModel());

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }
}
