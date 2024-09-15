using GitIssuesManager.Api.Models;
using GitIssuesManager.Logic.Models;

namespace GitIssuesManager.Api.Mappers;

public static class IssueCreateModelMapper
{
    public static GithubUpdateIssueModel ToGithubModel(this IssueUpdateModel model)
    {
        return new GithubUpdateIssueModel
        {
            Title = model.Name,
            Body = model.Description
        };
    }

    public static GitlabUpdateIssueModel ToGitlabModel(this IssueUpdateModel model)
    {
        return new GitlabUpdateIssueModel
        {
            Title = model.Name,
            Description = model.Description
        };
    }
}
