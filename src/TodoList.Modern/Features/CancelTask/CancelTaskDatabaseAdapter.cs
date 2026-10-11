using TodoList.Modern.Domain;
using TodoList.Modern.Infrastructure;

namespace TodoList.Modern.Features.CancelTask;

public sealed class CancelTaskDatabaseAdapter(InMemoryDatabase database) : ICancelTaskDatabasePort
{
    public Task<TodoItem?> GetByIdAsync(Guid id) => Task.FromResult(database.GetById(id));

    public Task UpdateAsync(TodoItem item)
    {
        database.Update(item);
        return Task.CompletedTask;
    }
}
