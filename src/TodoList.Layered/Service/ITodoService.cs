using TodoList.Layered.Domain;

namespace TodoList.Layered.Service;

public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> ListAsync();
    Task<Guid> CreateAsync(CreateTodoDto dto);
    Task CancelAsync(Guid id);
    Task CompleteAsync(Guid id);
}
