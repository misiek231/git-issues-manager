using GitIssuesManager.Logic.Models;
using Microsoft.AspNetCore.WebUtilities;
using OneOf;
using OneOf.Types;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Clients;

public class GitlabIssuesClient(IHttpClientFactory clientFactory) : GitIssuesClient(clientFactory)
{
    protected override GitIssueClientType ClientType => GitIssueClientType.Gitlab;

    public async Task<OneOf<ResultModel, Error>> CreateIssue(string projectId, GitlabUpdateIssueModel model)
    {
        var client = GetClient();

        var query = QueryHelpers.AddQueryString($"/api/v4/projects/{projectId}/issues", model.AsDictionary());

        var response = await client.PostAsync(query, null);

        if (response.IsSuccessStatusCode)
        {
            // TODO: return proper error
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }

    public async Task<OneOf<ResultModel, Error>> UpdateIssue(string projectId, string issueId, GitlabUpdateIssueModel model)
    {
        var client = GetClient();

        var query = QueryHelpers.AddQueryString($"/api/v4/projects/{projectId}/issues/{issueId}", model.AsDictionary());

        var response = await client.PutAsync(query, null);

        if (response.IsSuccessStatusCode)
        {
            // TODO: return proper error
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }


    public async Task<OneOf<ResultModel, Error>> CloseIssue(string projectId, string issueId)
    {
        var client = GetClient();

        var response = await client.PutAsync($"/api/v4/projects/{projectId}/issues/{issueId}?state_event=close", null);

        if (response.IsSuccessStatusCode)
        {
            // TODO: return proper error
            return await response.Content.ReadFromJsonAsync<ResultModel>() ?? new ResultModel();
        }

        return new Error();
    }
}
