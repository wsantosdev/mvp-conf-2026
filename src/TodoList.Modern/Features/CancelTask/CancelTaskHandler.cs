namespace TodoList.Modern.Features.CancelTask;

public sealed class CancelTaskHandler(ICancelTaskDatabasePort port)
{
    public async Task HandleAsync(CancelTaskCommand command)
    {
        var item = await port.GetByIdAsync(command.Id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {command.Id} não encontrada.");

        item.Cancel();
        await port.UpdateAsync(item);
    }
}
