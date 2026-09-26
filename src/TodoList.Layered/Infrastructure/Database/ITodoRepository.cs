using TodoList.Layered.Domain;

namespace TodoList.Layered.Infrastructure.Database;

public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> ListAsync();
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
}
