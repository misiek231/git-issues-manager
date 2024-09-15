using GitIssuesManager.Logic.Models;
using OneOf;
using OneOf.Types;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Clients;

public class GithubIssuesClient(IHttpClientFactory clientFactory) : GitIssuesClient(clientFactory)
{
    protected override GitIssueClientType ClientType => GitIssueClientType.Github;

    public async Task<OneOf<ResultModel, Error<string>>> CreateIssue(string owner, string repo, GithubUpdateIssueModel model)
    {
        var client = GetClient();

        try
        {
            var response = await client.PostAsJsonAsync($"/repos/{owner}/{repo}/issues", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
            }

            return new Error<string>(response.ReasonPhrase ?? "");
        }
        catch
        {
            return new Error<string>("Fatal error while requesting resource");
        }
    }

    public async Task<OneOf<ResultModel, Error<string>>> UpdateIssue(string owner, string repo, string issueNumber, GithubUpdateIssueModel model)
    {
        var client = GetClient();

        try
        {
            var response = await client.PatchAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
            }

            return new Error<string>(response.ReasonPhrase ?? "");
        }
        catch
        {
            return new Error<string>("Fatal error while requesting resource");
        }
    }

    public async Task<OneOf<ResultModel, Error<string>>> CloseIssue(string owner, string repo, string issueNumber)
    {
        var client = GetClient();

        try
        {
            var response = await client.PatchAsJsonAsync($"/repos/{owner}/{repo}/issues/{issueNumber}", new { State = "closed" });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
            }

            return new Error<string>(response.ReasonPhrase ?? "");
        }
        catch
        {
            return new Error<string>("Fatal error while requesting resource");
        }
    }
}
