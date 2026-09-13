using System.Collections.Concurrent;
using TodoList.Modern.Domain;
using TodoList.Modern.Features.CancelTask;
using TodoList.Modern.Features.CompleteTask;
using TodoList.Modern.Features.CreateTask;

namespace TodoList.Modern.Infrastructure;

public class TodoStorageAdapter :
    ICreateTaskPort,
    ICancelTaskPort,
    ICompleteTaskPort
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _db = new();

    public Task SaveAsync(TodoItem item)
    {
        _db[item.Id] = item;
        return Task.CompletedTask;
    }

    public Task<TodoItem?> GetByIdAsync(Guid id)
    {
        _db.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task UpdateAsync(TodoItem item)
    {
        _db[item.Id] = item;
        return Task.CompletedTask;
    }
}
