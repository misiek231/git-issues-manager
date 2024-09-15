using GitIssuesManager.Logic.Models;
using System.Text.Json;

namespace GitIssuesManager.Api.Models;

public class IssueCloseModel
{
    public required Dictionary<GitIssueClientType, object> Identifiers { get; set; }

    public static ValueTask<IssueCloseModel> BindAsync(HttpContext httpContext)
    {
        using var reader = new StreamReader(httpContext.Request.Body);
        var body = reader.ReadToEndAsync().Result;
        var json = JsonDocument.Parse(body).RootElement;

        var issueFormModel = new IssueCloseModel
        {
            Identifiers = []
        };

        foreach (var option in Enum.GetValues<GitIssueClientType>())
        {
            if (!json.TryGetProperty(option.ToString(), out JsonElement elem)) continue;

            issueFormModel.Identifiers[option] = option switch
            {
                GitIssueClientType.Github => elem.Deserialize<GithubIdentifier>() ?? new GithubIdentifier(),
                GitIssueClientType.Gitlab => elem.Deserialize<GitlabIdentifier>() ?? new GitlabIdentifier()
            };
        }

        return ValueTask.FromResult(issueFormModel);
    }

    public class GithubIdentifier
    {
        public string? Owner { get; set; }
        public string? Repo { get; set; }
        public string? IssueNumber { get; set; }
    }

    public class GitlabIdentifier
    {
        public string? ProjectId { get; set; }
        public string? IssueId { get; set; }
    }
}

