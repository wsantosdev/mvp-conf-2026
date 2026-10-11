using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CancelTask;

public interface ICancelTaskDatabasePort
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TodoItem item);
}
