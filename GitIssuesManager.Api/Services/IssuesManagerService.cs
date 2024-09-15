using GitIssuesManager.Api.Mappers;
using GitIssuesManager.Api.Models;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Models;
using OneOf;
using OneOf.Types;

namespace GitIssuesManager.Api.Services;

// TODO: unit tests
public class IssuesManagerService(GithubIssuesClient githubIssuesClient, GitlabIssuesClient gitlabIssuesClient)
{

    private readonly GithubIssuesClient githubIssuesClient = githubIssuesClient;
    private readonly GitlabIssuesClient gitlabIssuesClient = gitlabIssuesClient;

    public async Task<Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>> CreateIssue(IssueCreateModel model)
    {
        var agregatedResult = new Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>();

        foreach (var type in model.Identifiers.Keys)
        {
            var id = model.Identifiers[type];

            var result = type switch
            {
                GitIssueClientType.Github when id is IssueCreateModel.GithubIdentifier createId =>
                    await githubIssuesClient.CreateIssue(createId.Owner!, createId.Repo!, model.ToGithubModel()),

                GitIssueClientType.Gitlab when id is IssueCreateModel.GitlabIdentifier createId =>
                    await gitlabIssuesClient.CreateIssue(createId.ProjectId!, model.ToGitlabModel()),
            };

            agregatedResult.Add(type, result);

        }

        return agregatedResult;
    }

    public async Task<Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>> UpdateIssue(IssueUpdateModel model)
    {

        var agregatedResult = new Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>();

        foreach (var type in model.Identifiers.Keys)
        {
            var id = model.Identifiers[type];

            var result = type switch
            {
                GitIssueClientType.Github when id is IssueUpdateModel.GithubIdentifier updateId =>
                    await githubIssuesClient.UpdateIssue(updateId.Owner!, updateId.Repo!, updateId.IssueNumber!, model.ToGithubModel()),

                GitIssueClientType.Gitlab when id is IssueUpdateModel.GitlabIdentifier updateId =>
                    await gitlabIssuesClient.UpdateIssue(updateId.ProjectId!, updateId.IssueId!, model.ToGitlabModel()),
            };

            agregatedResult.Add(type, result);
        }

        return agregatedResult;
    }

    public async Task<Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>> CloseIssue(IssueCloseModel model)
    {

        var agregatedResult = new Dictionary<GitIssueClientType, OneOf<ResultModel, Error<string>>>();

        foreach (var type in model.Identifiers.Keys)
        {
            var id = model.Identifiers[type];

            var result = type switch
            {
                GitIssueClientType.Github when id is IssueCloseModel.GithubIdentifier closeId =>
                    await githubIssuesClient.CloseIssue(closeId.Owner!, closeId.Repo!, closeId.IssueNumber!),

                GitIssueClientType.Gitlab when id is IssueCloseModel.GitlabIdentifier closeId =>
                    await gitlabIssuesClient.CloseIssue(closeId.ProjectId!, closeId.IssueId!),
            };

            agregatedResult.Add(type, result);
        }

        return agregatedResult;
    }
}
