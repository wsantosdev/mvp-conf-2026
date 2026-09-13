using TodoList.NTier.Domain;

namespace TodoList.NTier.Infrastructure.Database;

public interface ITodoRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
}
