using FluentAssertions;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;
using Moq;
using OneOf.Types;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Test;

public class GithubIssueClientTests
{
    private readonly string owner = "misiek231";
    private readonly string repo = "git-issues-manager";
    private readonly string httpClientName = GitIssueClientType.Github.ToString();
    private readonly GitClientConfig config = new() { Url = "https://api.github.com", AuthToken = "Empty" };

    private readonly GithubIssuesClient sut;
    private readonly Mock<IHttpClientFactory> httpClientFactoryMock = new();
    private readonly MockHttpMessageHandler httpMessageHandlerMock = new();

    public GithubIssueClientTests()
    {
        sut = new GithubIssuesClient(httpClientFactoryMock.Object);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, typeof(ResultModel))]
    [InlineData(HttpStatusCode.BadRequest, typeof(Error))]
    public async Task CreateIssue_ShouldReturnProperResult_WhenRequestStatusCodeIs(HttpStatusCode statusCode, Type resultType)
    {
        // arrange
        var requestResult = new ResultModel() { Title = "test" };
        var createModel = new GithubUpdateIssueModel { Title = "Test", Body = "Test" };

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues")
            .Respond(statusCode, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient(httpClientName))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CreateIssue(owner, repo, createModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Value.Should().BeAssignableTo(resultType);
        actual.Switch(p => p.Should().BeEquivalentTo(requestResult), err => err.Should().BeAssignableTo<Error>());
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, typeof(ResultModel))]
    [InlineData(HttpStatusCode.BadRequest, typeof(Error))]
    public async Task UpdateIssue_ShouldReturnProperResult_WhenRequestStatusCodeIs(HttpStatusCode statusCode, Type resultType)
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };
        var updateModel = new GithubUpdateIssueModel { Title = "Test-edit", Body = "Test-edit" };

        httpMessageHandlerMock.When(HttpMethod.Patch, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(statusCode, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient(httpClientName))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.UpdateIssue(owner, repo, issueNumber, updateModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Value.Should().BeAssignableTo(resultType);
        actual.Switch(p => p.Should().BeEquivalentTo(requestResult), err => err.Should().BeAssignableTo<Error>());
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, typeof(ResultModel))]
    [InlineData(HttpStatusCode.BadRequest, typeof(Error))]
    public async Task CloseIssue_ShouldReturnProperResult_WhenRequestStatusCodeIs(HttpStatusCode statusCode, Type resultType)
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };

        httpMessageHandlerMock.When(HttpMethod.Patch, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(statusCode, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient(httpClientName))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CloseIssue(owner, repo, issueNumber);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Value.Should().BeAssignableTo(resultType);
        actual.Switch(p => p.Should().BeEquivalentTo(requestResult), err => err.Should().BeAssignableTo<Error>());
    }
}