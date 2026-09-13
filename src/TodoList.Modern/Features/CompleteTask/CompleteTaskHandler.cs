namespace TodoList.Modern.Features.CompleteTask;

public class CompleteTaskHandler
{
    private readonly ICompleteTaskPort _port;
    public CompleteTaskHandler(ICompleteTaskPort port) => _port = port;

    public async Task HandleAsync(CompleteTaskCommand command)
    {
        var item = await _port.GetByIdAsync(command.Id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {command.Id} não encontrada.");

        item.Complete();
        await _port.UpdateAsync(item);
    }
}
