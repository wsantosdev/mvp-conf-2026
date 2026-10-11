using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CompleteTask;

public interface ICompleteTaskDatabasePort
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TodoItem item);
}
