using GitIssuesManager.Api.MinimalApiExtensions;
using GitIssuesManager.Api.Models;
using GitIssuesManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitIssuesManager.Api.Endpoints;

// TODO: unit tests
public class IssuesEndpoints : IEndpointDefinition
{
    public string GroupName => "issues";

    public void DefineEndpoints(RouteGroupBuilder builder)
    {
        builder.MapPost("", CreateIssue);
        builder.MapPut("", UpdateIssue);
        builder.MapPut("close", CloseIssue);
    }

    public async Task<IResult> CreateIssue([FromServices] IssuesManagerService manager, IssueCreateModel model)
    {
        var result = await manager.CreateIssue(model);

        return Results.Ok(result.ToDictionary(p => p.Key, p => p.Value.Value));
    }

    public async Task<IResult> UpdateIssue([FromServices] IssuesManagerService manager, IssueUpdateModel model)
    {
        var result = await manager.UpdateIssue(model);

        return Results.Ok(result.ToDictionary(p => p.Key, p => p.Value.Value));
    }

    public async Task<IResult> CloseIssue([FromServices] IssuesManagerService manager, IssueCloseModel model)
    {
        var result = await manager.CloseIssue(model);

        return Results.Ok(result.ToDictionary(p => p.Key, p => p.Value.Value));
    }
}
