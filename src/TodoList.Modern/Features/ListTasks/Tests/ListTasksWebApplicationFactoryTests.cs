using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TodoList.Modern.Tests;

public class ListTasksWebApplicationFactoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ListTasksWebApplicationFactoryTests(WebApplicationFactory<Program> factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task ListTasksEndpoint_WithCreatedTask_ReturnsTask()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = "List me" });
        createResponse.EnsureSuccessStatusCode();
        using var createdTask = await JsonDocument.ParseAsync(await createResponse.Content.ReadAsStreamAsync());
        var id = createdTask.RootElement.GetProperty("id").GetGuid();

        var response = await _client.GetAsync("/api/v1/todos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var tasks = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Contains(tasks.RootElement.EnumerateArray(),
            task => task.GetProperty("id").GetGuid() == id
                    && task.GetProperty("title").GetString() == "List me");
    }
}
