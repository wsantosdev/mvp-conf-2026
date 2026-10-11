namespace TodoList.Modern.Features.CompleteTask;

public sealed class CompleteTaskHandler(ICompleteTaskDatabasePort port)
{
    public async Task HandleAsync(CompleteTaskCommand command)
    {
        var item = await port.GetByIdAsync(command.Id)
                   ?? throw new KeyNotFoundException($"Tarefa com ID {command.Id} não encontrada.");

        item.Complete();
        await port.UpdateAsync(item);
    }
}
