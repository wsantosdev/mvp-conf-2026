using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CreateTask;

public sealed class CreateTaskHandler(ICreateTaskDatabasePort port)
{
    public async Task<Guid> HandleAsync(CreateTaskCommand command)
    {
        var item = new TodoItem(command.Title);
        await port.SaveAsync(item);
        return item.Id;
    }
}
