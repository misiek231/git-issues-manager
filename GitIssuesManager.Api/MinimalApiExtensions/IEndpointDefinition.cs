namespace GitIssuesManager.Api.MinimalApiExtensions;

public interface IEndpointDefinition
{
    string GroupName { get; }
    void DefineEndpoints(RouteGroupBuilder services);
}
