using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CancelTask;

public interface ICancelTaskPort
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TodoItem item);
}
