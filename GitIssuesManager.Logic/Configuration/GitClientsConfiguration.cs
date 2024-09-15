using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace GitIssuesManager.Logic.Configuration;

public class GitClientConfiguration
{
    [Required]
    public required string Url { get; set; }

    [Required]
    public required string AuthToken { get; set; }
}

public class GitClientsConfiguration
{
    [Required]
    [ValidateEnumeratedItems]
    public required Dictionary<GitIssueClientType, GitClientConfiguration> GitClients { get; set; }

    [ValidateEnumeratedItems]
    public IEnumerable<GitClientConfiguration> MyOptionsValues => GitClients.Values;

    public bool Validate()
    {
        return Enum.GetValues<GitIssueClientType>().All(GitClients.ContainsKey);
    }
}