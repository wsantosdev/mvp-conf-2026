using System.Collections.Concurrent;
using TodoList.NTier.Domain;

namespace TodoList.NTier.Infrastructure.Database;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _db = new();
    public Task<TodoItem?> GetByIdAsync(Guid id)
    {
        _db.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }
    public Task AddAsync(TodoItem item)
    {
        _db[item.Id] = item;
        return Task.CompletedTask;
    }
    public Task UpdateAsync(TodoItem item)
    {
        _db[item.Id] = item;
        return Task.CompletedTask;
    }
}
