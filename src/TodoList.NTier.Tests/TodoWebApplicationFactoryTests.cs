using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TodoList.NTier.Tests;

public class TodoWebApplicationFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public TodoWebApplicationFactoryTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CreateEndpoint_WithValidTitle_ReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = "Buy groceries" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.NotEqual(Guid.Empty, body.RootElement.GetProperty("id").GetGuid());
    }

    [Fact]
    public async Task CreateEndpoint_WithEmptyTitle_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CancelEndpoint_WithCreatedTodo_ReturnsNoContent()
    {
        var id = await CreateTodoAsync("Cancel me");
        Assert.Equal(HttpStatusCode.NoContent, (await _client.PostAsync($"/api/v1/todos/{id}/cancel", null)).StatusCode);
    }

    [Fact]
    public async Task CancelEndpoint_WithUnknownTodo_ReturnsNotFound()
    {
        var response = await _client.PostAsync($"/api/v1/todos/{Guid.NewGuid()}/cancel", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelEndpoint_WithCompletedTodo_ReturnsBadRequest()
    {
        var id = await CreateTodoAsync("Complete then cancel");
        Assert.Equal(HttpStatusCode.NoContent, (await _client.PostAsync($"/api/v1/todos/{id}/complete", null)).StatusCode);
        var response = await _client.PostAsync($"/api/v1/todos/{id}/cancel", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CompleteEndpoint_WithCreatedTodo_ReturnsNoContent()
    {
        var id = await CreateTodoAsync("Complete me");
        Assert.Equal(HttpStatusCode.NoContent, (await _client.PostAsync($"/api/v1/todos/{id}/complete", null)).StatusCode);
    }

    [Fact]
    public async Task CompleteEndpoint_WithUnknownTodo_ReturnsNotFound()
    {
        var response = await _client.PostAsync($"/api/v1/todos/{Guid.NewGuid()}/complete", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteEndpoint_WithCancelledTodo_ReturnsBadRequest()
    {
        var id = await CreateTodoAsync("Cancel then complete");
        Assert.Equal(HttpStatusCode.NoContent, (await _client.PostAsync($"/api/v1/todos/{id}/cancel", null)).StatusCode);
        var response = await _client.PostAsync($"/api/v1/todos/{id}/complete", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<Guid> CreateTodoAsync(string title)
    {
        var response = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = title });
        response.EnsureSuccessStatusCode();
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        return body.RootElement.GetProperty("id").GetGuid();
    }
}
