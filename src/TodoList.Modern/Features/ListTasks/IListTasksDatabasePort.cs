using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.ListTasks;

public interface IListTasksDatabasePort
{
    Task<IReadOnlyList<TodoItem>> ListAsync();
}
