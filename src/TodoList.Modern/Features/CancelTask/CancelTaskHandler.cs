namespace TodoList.Modern.Features.CancelTask;

public class CancelTaskHandler
{
    private readonly ICancelTaskPort _port;
    public CancelTaskHandler(ICancelTaskPort port) => _port = port;

    public async Task HandleAsync(CancelTaskCommand command)
    {
        var item = await _port.GetByIdAsync(command.Id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {command.Id} não encontrada.");

        item.Cancel();
        await _port.UpdateAsync(item);
    }
}
