using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TodoList.Modern.Tests;

public class CompleteTaskWebApplicationFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public CompleteTaskWebApplicationFactoryTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CompleteTaskEndpoint_WithCreatedTodo_ReturnsNoContent()
    {
        var id = await CreateTodoAsync("Complete me");
        var response = await _client.PostAsync($"/api/v1/todos/{id}/complete", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task CompleteTaskEndpoint_WithUnknownTodo_ReturnsNotFound()
    {
        var response = await _client.PostAsync($"/api/v1/todos/{Guid.NewGuid()}/complete", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteTaskEndpoint_WithCancelledTodo_ReturnsBadRequest()
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
