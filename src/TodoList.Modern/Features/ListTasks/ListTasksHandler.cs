using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.ListTasks;

public class ListTasksHandler
{
    private readonly IListTasksPort _port;

    public ListTasksHandler(IListTasksPort port) => _port = port;

    public Task<IReadOnlyList<TodoItem>> HandleAsync() => _port.ListAsync();
}
