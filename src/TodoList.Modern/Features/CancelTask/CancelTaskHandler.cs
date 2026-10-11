namespace TodoList.Modern.Features.CancelTask;

public class CancelTaskHandler
{
    private readonly ICancelTaskDatabasePort _port;

    public CancelTaskHandler(ICancelTaskDatabasePort port) => _port = port;

    public async Task HandleAsync(CancelTaskCommand command)
    {
        var item = await _port.GetByIdAsync(command.Id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {command.Id} não encontrada.");

        item.Cancel();
        await _port.UpdateAsync(item);
    }
}
