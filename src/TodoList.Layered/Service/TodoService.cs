using TodoList.Layered.Domain;
using TodoList.Layered.Infrastructure.Database;

namespace TodoList.Layered.Service;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    public TodoService(ITodoRepository repository) => _repository = repository;

    public Task<IReadOnlyList<TodoItem>> ListAsync() => _repository.ListAsync();

    public async Task<Guid> CreateAsync(CreateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("O título da tarefa não pode ser vazio.", nameof(dto.Title));
        var item = new TodoItem { Id = Guid.NewGuid(), Title = dto.Title, IsCompleted = false, IsCancelled = false };
        await _repository.AddAsync(item);
        return item.Id;
    }

    public async Task CancelAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
        if (item.IsCompleted)
            throw new InvalidOperationException("Não é possível cancelar uma tarefa já concluída.");
        item.IsCancelled = true;
        await _repository.UpdateAsync(item);
    }

    public async Task CompleteAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
        if (item.IsCancelled)
            throw new InvalidOperationException("Não é possível concluir uma tarefa já cancelada.");
        item.IsCompleted = true;
        await _repository.UpdateAsync(item);
    }
}
