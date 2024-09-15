using GitIssuesManager.Logic.Models;
using System.Text.Json;

namespace GitIssuesManager.Api.Models;

public class IssueUpdateModel
{
    public required string? Name { get; set; }
    public required string? Description { get; set; }
    public required Dictionary<GitIssueClientType, object> Identifiers { get; set; }

    public static ValueTask<IssueUpdateModel> BindAsync(HttpContext httpContext)
    {
        using var reader = new StreamReader(httpContext.Request.Body);
        var body = reader.ReadToEndAsync().Result;
        var json = JsonDocument.Parse(body).RootElement;

        var issueFormModel = new IssueUpdateModel
        {
            Name = json.GetProperty(nameof(Name)).GetString() ?? "",
            Description = json.GetProperty(nameof(Description)).GetString() ?? "",
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

