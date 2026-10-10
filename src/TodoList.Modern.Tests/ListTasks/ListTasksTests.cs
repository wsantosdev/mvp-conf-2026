using TodoList.Modern.Domain;
using TodoList.Modern.Features.ListTasks;
using TodoList.Modern.Infrastructure;
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

    [Fact]
    public async Task ListAsync_WithNoTodos_ReturnsEmptyList()
    {
        var adapter = new InMemoryListTasksAdapter(new InMemoryDatabase());

        var todos = await adapter.ListAsync();

        Assert.Empty(todos);
    }

    [Fact]
    public async Task ListAsync_WithSavedTodos_ReturnsAllTodos()
    {
        var database = new InMemoryDatabase();
        var firstTodo = new TodoItem("First");
        var secondTodo = new TodoItem("Second");
        database.Save(firstTodo);
        database.Save(secondTodo);
        var adapter = new InMemoryListTasksAdapter(database);

        var todos = await adapter.ListAsync();

        Assert.Equal(2, todos.Count);
        Assert.Contains(firstTodo, todos);
        Assert.Contains(secondTodo, todos);
    }

    private sealed class TestListTasksPort(IReadOnlyList<TodoItem> tasks) : IListTasksPort
    {
        public Task<IReadOnlyList<TodoItem>> ListAsync() => Task.FromResult(tasks);
    }
}
