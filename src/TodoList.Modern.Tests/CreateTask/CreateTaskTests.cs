using TodoList.Modern.Domain;
using TodoList.Modern.Features.CreateTask;
using Xunit;

namespace TodoList.Modern.Tests;

public class CreateTaskTests
{
    [Fact]
    public async Task HandleAsync_WithValidTitle_SavesPendingTodo()
    {
        var port = new TestCreateTaskPort();
        var handler = new CreateTaskHandler(port);
        var id = await handler.HandleAsync(new CreateTaskCommand("Buy groceries"));
        Assert.NotNull(port.SavedItem);
        Assert.Equal(id, port.SavedItem.Id);
        Assert.Equal("Buy groceries", port.SavedItem.Title);
        Assert.False(port.SavedItem.IsCompleted);
        Assert.False(port.SavedItem.IsCancelled);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task HandleAsync_WithEmptyTitle_ThrowsArgumentException(string title)
    {
        var port = new TestCreateTaskPort();
        var handler = new CreateTaskHandler(port);
        await Assert.ThrowsAsync<ArgumentException>(
            () => handler.HandleAsync(new CreateTaskCommand(title)));
        Assert.Null(port.SavedItem);
    }

    private sealed class TestCreateTaskPort : ICreateTaskPort
    {
        public TodoItem? SavedItem { get; private set; }
        public Task SaveAsync(TodoItem item)
        {
            SavedItem = item;
            return Task.CompletedTask;
        }
    }
}
