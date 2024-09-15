using FluentAssertions;
using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.Options;
using Moq;
using OneOf;
using OneOf.Types;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace GitIssuesManager.Logic.Test;

public class GithubIssueClientTests
{
    private readonly string owner = "misiek231";
    private readonly string repo = "git-issues-manager";
    private readonly GitClientConfig config = new() { Url = "https://api.github.com" };
    
    private readonly GithubIssuesClient sut;
    private readonly Mock<IHttpClientFactory> httpClientFactoryMock = new();
    private readonly Mock<IOptions<GitClientConfig>> optionsMock = new();
    private readonly MockHttpMessageHandler httpMessageHandlerMock = new();

    public GithubIssueClientTests()
    {
        sut = new GithubIssuesClient(optionsMock.Object, httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task CreateIssue_ShouldReturnSuccessResult_WhenRequestStatusCodeIsSuccess()
    {
        // arrange
        var requestResult = new ResultModel() { Title = "test" };
        var createModel = new CreateIssueModel { Title = "Test", Body = "Test" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues")
            .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CreateIssue(owner, repo, createModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT0);
        actual.AsT0.Should().BeEquivalentTo(requestResult);
    }

    [Fact]
    public async Task CreateIssue_ShouldReturnError_WhenRequestStatusCodeIsNotSuccess()
    {
        // arrange
        var requestResult = new ResultModel() { Title = "test" };
        var createModel = new CreateIssueModel { Title = "Test", Body = "Test" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues")
            .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CreateIssue(owner, repo, createModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT1);
        //actual.AsT1.Should().BeEquivalentTo(requestResult);
    }

    [Fact]
    public async Task UpdateIssue_ShouldReturnSuccessResult_WhenRequestStatusCodeIsSuccess()
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };
        var updateModel = new UpdateIssueModel { Title = "Test-edit", Body = "Test-edit" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.UpdateIssue(owner, repo, issueNumber, updateModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT0);
        actual.AsT0.Should().BeEquivalentTo(requestResult);
    }

    [Fact]
    public async Task UpdateIssue_ShouldReturnError_WhenRequestStatusCodeIsNotSuccess()
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };
        var updateModel = new UpdateIssueModel { Title = "Test-edit", Body = "Test-edit" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.UpdateIssue(owner, repo, issueNumber, updateModel);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT1);
        //actual.AsT1.Should().BeEquivalentTo(requestResult);
    }

    [Fact]
    public async Task CloseIssue_ShouldReturnSuccessResult_WhenRequestStatusCodeIsSuccess()
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CloseIssue(owner, repo, issueNumber);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT0);
        actual.AsT0.Should().BeEquivalentTo(requestResult);
    }

    [Fact]
    public async Task CloseIssue_ShouldReturnError_WhenRequestStatusCodeIsNotSuccess()
    {
        // arrange
        var issueNumber = "3";
        var requestResult = new ResultModel() { Title = "test" };

        optionsMock.Setup(p => p.Value)
            .Returns(config);

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://api.github.com/repos/{owner}/{repo}/issues/{issueNumber}")
            .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GithubClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CloseIssue(owner, repo, issueNumber);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT1);
        //actual.AsT1.Should().BeEquivalentTo(requestResult);
    }
}