using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.ListTasks;

public sealed class ListTasksHandler(IListTasksDatabasePort port)
{
    public Task<IReadOnlyList<TodoItem>> HandleAsync() => port.ListAsync();
}
