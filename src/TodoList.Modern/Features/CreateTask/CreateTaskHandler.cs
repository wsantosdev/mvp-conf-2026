using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CreateTask;

public class CreateTaskHandler
{
    private readonly ICreateTaskPort _port;
    public CreateTaskHandler(ICreateTaskPort port) => _port = port;

    public async Task<Guid> HandleAsync(CreateTaskCommand command)
    {
        var item = new TodoItem(command.Title);
        await _port.SaveAsync(item);
        return item.Id;
    }
}
