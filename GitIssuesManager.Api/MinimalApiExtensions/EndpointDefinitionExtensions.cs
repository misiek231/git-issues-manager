namespace GitIssuesManager.Api.MinimalApiExtensions;

public static class EndpointDefinitionExtensions
{
    public static void AddEndpointDefinitions(this IServiceCollection services)
    {
        var defs = typeof(EndpointDefinitionExtensions).Assembly.ExportedTypes
            .Where(p => p.IsAssignableTo(typeof(IEndpointDefinition)) && p.IsClass)
            .ToList();

        foreach (var def in defs)
        {
            services.AddScoped(typeof(IEndpointDefinition), def);
        }
    }

    public static void UseEndpointDefinitions(this WebApplication app, string? apiPrefix = null)
    {
        using var services = app.Services.CreateScope();
        var defs = services.ServiceProvider.GetServices<IEndpointDefinition>();

        foreach (var def in defs)
        {
            def.DefineEndpoints(app.MapGroup($"{apiPrefix}/{def.GroupName}"));
        }
    }
}
