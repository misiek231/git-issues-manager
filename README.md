# Git issues manager
Simple .NET web api for managing issues in some most popular hosting platforms such as github. App was written in such way to make it easy to add other clients.

### Currently supported actions

- create issue (name and description)
- update issue (name and description)
- close issue

### Currently supported clients

- Github
- Gitlab

## Running project

1. Pull repository
2. Duplicate file `appsettings.Example.json` and rename it to: `appsettings.json`
3. Set variable `GitClients` inside appsettings.json (all props are required for proper app working)
4. Run project `GitIssuesManager.Api` in Visual Studio or with command line  

```bash
dotnet run --project GitIssuesManager.Api
```

Warning! .Net SDK or runtime in version 8.0 or higher is required to run project.

## Example of usage

Create new issue by calling:

```http
POST {HOST}/api/issues
Accept: application/json
Content-Type: application/json
{
    # New issue params
    "Name": "test", 
    "Description": "Test", 
    
    # Optional configuration for both platforms depending on where you want to create issue
    "Github": { 
        "Owner": "misiek231",
        "Repo": "git-issues-manager"
    },
    "Gitlab": {
        "ProjectId": "test"
    }
}
```

You can find another examples of usage and test api in file `GitIssuesManager.Api.http`