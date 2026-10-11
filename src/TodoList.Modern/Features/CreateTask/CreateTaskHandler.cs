using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CreateTask;

public class CreateTaskHandler
{
    private readonly ICreateTaskDatabasePort _port;

    public CreateTaskHandler(ICreateTaskDatabasePort port) => _port = port;

    public async Task<Guid> HandleAsync(CreateTaskCommand command)
    {
        var item = new TodoItem(command.Title);
        await _port.SaveAsync(item);
        return item.Id;
    }
}
