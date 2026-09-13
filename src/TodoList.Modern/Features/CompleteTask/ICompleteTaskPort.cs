using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CompleteTask;

public interface ICompleteTaskPort
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TodoItem item);
}
