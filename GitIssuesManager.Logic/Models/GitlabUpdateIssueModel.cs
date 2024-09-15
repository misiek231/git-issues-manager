namespace GitIssuesManager.Logic.Models;

public class GitlabUpdateIssueModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }

    public Dictionary<string, string?> AsDictionary() => new()
    {
        { nameof(Title).ToLower(), Title },
        { nameof(Description).ToLower(), Description },
    };
}
