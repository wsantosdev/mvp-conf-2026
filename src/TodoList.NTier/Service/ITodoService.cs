namespace TodoList.NTier.Service;

public interface ITodoService
{
    Task<Guid> CreateAsync(CreateTodoDto dto);
    Task CancelAsync(Guid id);
    Task CompleteAsync(Guid id);
}
