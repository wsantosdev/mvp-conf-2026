using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TodoList.Modern.Tests;

public class CreateTaskWebApplicationFactoryTests : WebApplicationFactory<Program>
{
    private readonly HttpClient _client;
    public CreateTaskWebApplicationFactoryTests() => _client = CreateClient();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var contentRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        builder.UseContentRoot(contentRoot);
    }

    [Fact]
    public async Task CreateTaskEndpoint_WithValidTitle_ReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = "Buy groceries" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains("/api/v1/todos/", response.Headers.Location.ToString());
        using var body = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.NotEqual(Guid.Empty, body.RootElement.GetProperty("id").GetGuid());
    }

    [Fact]
    public async Task CreateTaskEndpoint_WithEmptyTitle_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/todos", new { Title = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
