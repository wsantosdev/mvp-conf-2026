using System.Collections.Concurrent;
using TodoList.Modern.Domain;

namespace TodoList.Modern.Infrastructure;

public sealed class InMemoryDatabase
{
    private readonly ConcurrentDictionary<Guid, TodoItem> _items = new();

    public void Save(TodoItem item) => _items[item.Id] = item;

    public TodoItem? GetById(Guid id)
    {
        _items.TryGetValue(id, out var item);
        return item;
    }

    public void Update(TodoItem item) => _items[item.Id] = item;
}
