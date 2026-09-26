using TodoList.Modern.Domain;
using TodoList.Modern.Features.ListTasks;
using Xunit;

namespace TodoList.Modern.Tests;

public class ListTasksTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllTasksFromPort()
    {
        var tasks = new[]
        {
            new TodoItem("First"),
            new TodoItem("Second")
        };
        var handler = new ListTasksHandler(new TestListTasksPort(tasks));

        var result = await handler.HandleAsync();

        Assert.Same(tasks, result);
    }

    private sealed class TestListTasksPort(IReadOnlyList<TodoItem> tasks) : IListTasksPort
    {
        public Task<IReadOnlyList<TodoItem>> ListAsync() => Task.FromResult(tasks);
    }
}
