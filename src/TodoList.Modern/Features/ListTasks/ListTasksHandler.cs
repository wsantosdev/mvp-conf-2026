using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.ListTasks;

public class ListTasksHandler
{
    private readonly IListTasksDatabasePort _port;

    public ListTasksHandler(IListTasksDatabasePort port) => _port = port;

    public Task<IReadOnlyList<TodoItem>> HandleAsync() => _port.ListAsync();
}
