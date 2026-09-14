using TodoList.Layered.Domain;

namespace TodoList.Layered.Infrastructure.Database;

public interface ITodoRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
}
