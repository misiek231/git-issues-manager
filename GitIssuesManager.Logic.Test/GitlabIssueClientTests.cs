using FluentAssertions;
using GitIssuesManager.Logic.Clients;
using GitIssuesManager.Logic.Configuration;
using GitIssuesManager.Logic.Models;
using Microsoft.Extensions.Logging;
using Moq;
using OneOf;
using OneOf.Types;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace GitIssuesManager.Logic.Test;

public class GitlabIssueClientTests
{
    private readonly string projectId = "1";
    private readonly GitClientConfig config = new() { Url = "https://gitlab.example.com" };

    private readonly GitlabIssuesClient sut;
    private readonly Mock<IHttpClientFactory> httpClientFactoryMock = new();
    private readonly MockHttpMessageHandler httpMessageHandlerMock = new();

    public GitlabIssueClientTests()
    {
        sut = new GitlabIssuesClient(httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task CreateIssue_ShouldReturnSuccessResult_WhenRequestStatusCodeIsSuccess()
    {
        // arrange
        var requestResult = new ResultModel() { Title = "test" };
        var createModel = new GitlabUpdateIssueModel { Title = "Issues with auth", Description = "Test description" };

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues?title=Issues%20with%20auth&description=Test%20description")
            .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CreateIssue(projectId, createModel);

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
        var createModel = new GitlabUpdateIssueModel { Title = "Issues with auth", Description = "Test description" };

        httpMessageHandlerMock.When(HttpMethod.Post, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues?title=Issues%20with%20auth&description=Test%20description")
           .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CreateIssue(projectId, createModel);

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
        var issueId = "3";
        var requestResult = new ResultModel() { Title = "test" };
        var updateModel = new GitlabUpdateIssueModel { Title = "Issues with auth", Description = "Test description" };

        httpMessageHandlerMock.When(HttpMethod.Put, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues/{issueId}?title=Issues%20with%20auth&description=Test%20description")
           .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.UpdateIssue(projectId, issueId, updateModel);

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
        var issueId = "3";
        var requestResult = new ResultModel() { Title = "test" };
        var updateModel = new GitlabUpdateIssueModel { Title = "Issues with auth", Description = "Test description" };

        httpMessageHandlerMock.When(HttpMethod.Put, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues/{issueId}?title=Issues%20with%20auth&description=Test%20description")
           .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.UpdateIssue(projectId, issueId, updateModel);

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
        var issueId = "3";
        var requestResult = new ResultModel() { Title = "test" };

        httpMessageHandlerMock.When(HttpMethod.Put, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues/{issueId}?state_event=close")
           .Respond(HttpStatusCode.OK, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CloseIssue(projectId, issueId);

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
        var issueId = "3";
        var requestResult = new ResultModel() { Title = "test" };

        httpMessageHandlerMock.When(HttpMethod.Put, $"https://gitlab.example.com/api/v4/projects/{projectId}/issues/{issueId}?state_event=close")
           .Respond(HttpStatusCode.BadRequest, JsonContent.Create(requestResult));

        httpClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Invalid client name was provided"));

        httpClientFactoryMock.Setup(p => p.CreateClient("GitlabClient"))
            .Returns(new HttpClient(httpMessageHandlerMock) { BaseAddress = new Uri(config.Url!) })
            .Verifiable();

        // act 
        var actual = await sut.CloseIssue(projectId, issueId);

        // assert
        httpClientFactoryMock.Verify();
        httpMessageHandlerMock.VerifyNoOutstandingRequest();
        actual.Should().Match<OneOf<ResultModel, Error>>(p => p.IsT1);
        //actual.AsT1.Should().BeEquivalentTo(requestResult);
    }
}