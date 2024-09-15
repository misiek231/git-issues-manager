using GitIssuesManager.Logic.Models;
using System.Text.Json;

namespace GitIssuesManager.Api.Models;

// TODO: Implement validation of models
public class IssueCreateModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Dictionary<GitIssueClientType, object> Identifiers { get; set; }

    public static ValueTask<IssueCreateModel> BindAsync(HttpContext httpContext)
    {
        using var reader = new StreamReader(httpContext.Request.Body);
        var body = reader.ReadToEndAsync().Result;
        var json = JsonDocument.Parse(body).RootElement;

        var issueFormModel = new IssueCreateModel
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
    }

    public class GitlabIdentifier
    {
        public string? ProjectId { get; set; }
    }
}
