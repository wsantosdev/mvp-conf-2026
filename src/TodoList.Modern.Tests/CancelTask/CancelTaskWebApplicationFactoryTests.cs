using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TodoList.Modern.Tests;

public class CancelTaskWebApplicationFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public CancelTaskWebApplicationFactoryTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CancelTaskEndpoint_WithCreatedTodo_ReturnsNoContent()
    {
        var id = await CreateTodoAsync("Cancel me");
        var response = await _client.PostAsync($"/api/v1/todos/{id}/cancel", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task CancelTaskEndpoint_WithUnknownTodo_ReturnsNotFound()
    {
        var response = await _client.PostAsync($"/api/v1/todos/{Guid.NewGuid()}/cancel", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CancelTaskEndpoint_WithCompletedTodo_ReturnsBadRequest()
    {
        var id = await CreateTodoAsync("Complete then cancel");
        Assert.Equal(HttpStatusCode.NoContent, (await _client.PostAsync($"/api/v1/todos/{id}/complete", null)).StatusCode);
        var response = await _client.PostAsync($"/api/v1/todos/{id}/cancel", null);
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
