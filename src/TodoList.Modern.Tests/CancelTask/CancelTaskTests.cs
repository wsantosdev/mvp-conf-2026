using TodoList.Modern.Domain;
using TodoList.Modern.Features.CancelTask;
using Xunit;

namespace TodoList.Modern.Tests;

public class CancelTaskTests
{
    [Fact]
    public async Task HandleAsync_WithPendingTodo_CancelsTodo()
    {
        var todo = new TodoItem("Cancel me");
        var port = new TestTaskPort(todo);
        await new CancelTaskHandler(port).HandleAsync(new CancelTaskCommand(todo.Id));
        Assert.True(todo.IsCancelled);
        Assert.False(todo.IsCompleted);
        Assert.Same(todo, port.UpdatedItem);
    }

    [Fact]
    public async Task HandleAsync_WithUnknownTodo_ThrowsKeyNotFoundException()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => new CancelTaskHandler(new TestTaskPort()).HandleAsync(new CancelTaskCommand(Guid.NewGuid())));
    }

    [Fact]
    public async Task HandleAsync_WithCompletedTodo_ThrowsInvalidOperationException()
    {
        var todo = new TodoItem("Already complete");
        todo.Complete();
        var port = new TestTaskPort(todo);
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => new CancelTaskHandler(port).HandleAsync(new CancelTaskCommand(todo.Id)));
        Assert.Null(port.UpdatedItem);
    }

    private sealed class TestTaskPort : ICancelTaskPort
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
