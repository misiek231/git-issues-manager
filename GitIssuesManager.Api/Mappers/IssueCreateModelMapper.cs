using GitIssuesManager.Api.Models;
using GitIssuesManager.Logic.Models;

namespace GitIssuesManager.Api.Mappers;

public static class IssueUpdateModelMapper
{
    public static GithubUpdateIssueModel ToGithubModel(this IssueCreateModel model)
    {
        return new GithubUpdateIssueModel
        {
            Title = model.Name,
            Body = model.Description
        };
    }

    public static GitlabUpdateIssueModel ToGitlabModel(this IssueCreateModel model)
    {
        return new GitlabUpdateIssueModel
        {
            Title = model.Name,
            Description = model.Description
        };
    }
}
