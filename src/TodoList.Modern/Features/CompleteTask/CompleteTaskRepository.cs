using TodoList.Modern.Domain;
using TodoList.Modern.Infrastructure;

namespace TodoList.Modern.Features.CompleteTask;

public sealed class CompleteTaskRepository(InMemoryDatabase database) : ICompleteTaskDatabasePort
{
    public Task<TodoItem?> GetByIdAsync(Guid id) => Task.FromResult(database.GetById(id));

    public Task UpdateAsync(TodoItem item)
    {
        database.Update(item);
        return Task.CompletedTask;
    }
}
