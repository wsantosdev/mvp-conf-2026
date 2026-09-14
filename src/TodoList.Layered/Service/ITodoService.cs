namespace TodoList.Layered.Service;

public interface ITodoService
{
    Task<Guid> CreateAsync(CreateTodoDto dto);
    Task CancelAsync(Guid id);
    Task CompleteAsync(Guid id);
}
