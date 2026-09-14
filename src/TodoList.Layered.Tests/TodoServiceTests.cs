using TodoList.Layered.Domain;
using TodoList.Layered.Infrastructure.Database;
using TodoList.Layered.Service;
using Xunit;

namespace TodoList.Layered.Tests;

public class TodoServiceTests
{
    [Fact]
    public async Task CreateAsync_WithValidTitle_CreatesPendingTodo()
    {
        var repository = new TestTodoRepository();
        var id = await new TodoService(repository).CreateAsync(new CreateTodoDto("Buy groceries"));
        var created = await repository.GetByIdAsync(id);
        Assert.NotNull(created);
        Assert.Equal(id, created.Id);
        Assert.Equal("Buy groceries", created.Title);
        Assert.False(created.IsCompleted);
        Assert.False(created.IsCancelled);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task CreateAsync_WithEmptyTitle_ThrowsArgumentException(string title)
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => new TodoService(new TestTodoRepository()).CreateAsync(new CreateTodoDto(title)));
        Assert.Equal("Title", exception.ParamName);
    }

    [Fact]
    public async Task CancelAsync_WithPendingTodo_MarksTodoAsCancelled()
    {
        var repository = new TestTodoRepository();
        var todo = new TodoItem { Id = Guid.NewGuid(), Title = "Cancel me" };
        await repository.AddAsync(todo);
        await new TodoService(repository).CancelAsync(todo.Id);
        Assert.True(todo.IsCancelled);
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public async Task CancelAsync_WithUnknownTodo_ThrowsKeyNotFoundException() =>
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => new TodoService(new TestTodoRepository()).CancelAsync(Guid.NewGuid()));

    [Fact]
    public async Task CancelAsync_WithCompletedTodo_ThrowsInvalidOperationException()
    {
        var repository = new TestTodoRepository();
        var todo = new TodoItem { Id = Guid.NewGuid(), Title = "Complete", IsCompleted = true };
        await repository.AddAsync(todo);
        await Assert.ThrowsAsync<InvalidOperationException>(() => new TodoService(repository).CancelAsync(todo.Id));
        Assert.False(todo.IsCancelled);
    }

    [Fact]
    public async Task CompleteAsync_WithPendingTodo_MarksTodoAsCompleted()
    {
        var repository = new TestTodoRepository();
        var todo = new TodoItem { Id = Guid.NewGuid(), Title = "Complete me" };
        await repository.AddAsync(todo);
        await new TodoService(repository).CompleteAsync(todo.Id);
        Assert.True(todo.IsCompleted);
        Assert.False(todo.IsCancelled);
    }

    [Fact]
    public async Task CompleteAsync_WithUnknownTodo_ThrowsKeyNotFoundException() =>
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => new TodoService(new TestTodoRepository()).CompleteAsync(Guid.NewGuid()));

    [Fact]
    public async Task CompleteAsync_WithCancelledTodo_ThrowsInvalidOperationException()
    {
        var repository = new TestTodoRepository();
        var todo = new TodoItem { Id = Guid.NewGuid(), Title = "Cancelled", IsCancelled = true };
        await repository.AddAsync(todo);
        await Assert.ThrowsAsync<InvalidOperationException>(() => new TodoService(repository).CompleteAsync(todo.Id));
        Assert.False(todo.IsCompleted);
    }

    private sealed class TestTodoRepository : ITodoRepository
    {
        private readonly Dictionary<Guid, TodoItem> _items = new();
        public Task<TodoItem?> GetByIdAsync(Guid id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }
        public Task AddAsync(TodoItem item)
        {
            _items.Add(item.Id, item);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(TodoItem item)
        {
            _items[item.Id] = item;
            return Task.CompletedTask;
        }
    }
}
