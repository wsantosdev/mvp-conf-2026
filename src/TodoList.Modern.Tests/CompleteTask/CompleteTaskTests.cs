using TodoList.Modern.Domain;
using TodoList.Modern.Features.CompleteTask;
using Xunit;

namespace TodoList.Modern.Tests;

public class CompleteTaskTests
{
    [Fact]
    public async Task HandleAsync_WithPendingTodo_CompletesTodo()
    {
        var todo = new TodoItem("Complete me");
        var port = new TestTaskPort(todo);
        await new CompleteTaskHandler(port).HandleAsync(new CompleteTaskCommand(todo.Id));
        Assert.True(todo.IsCompleted);
        Assert.False(todo.IsCancelled);
        Assert.Same(todo, port.UpdatedItem);
    }

    [Fact]
    public async Task HandleAsync_WithUnknownTodo_ThrowsKeyNotFoundException()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => new CompleteTaskHandler(new TestTaskPort()).HandleAsync(new CompleteTaskCommand(Guid.NewGuid())));
    }

    [Fact]
    public async Task HandleAsync_WithCancelledTodo_ThrowsInvalidOperationException()
    {
        var todo = new TodoItem("Already cancelled");
        todo.Cancel();
        var port = new TestTaskPort(todo);
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => new CompleteTaskHandler(port).HandleAsync(new CompleteTaskCommand(todo.Id)));
        Assert.Null(port.UpdatedItem);
    }

    private sealed class TestTaskPort : ICompleteTaskPort
    {
        private readonly TodoItem? _item;
        public TodoItem? UpdatedItem { get; private set; }
        public TestTaskPort(TodoItem? item = null) => _item = item;
        public Task<TodoItem?> GetByIdAsync(Guid id) => Task.FromResult(_item?.Id == id ? _item : null);
        public Task UpdateAsync(TodoItem item)
        {
            UpdatedItem = item;
            return Task.CompletedTask;
        }
    }
}
