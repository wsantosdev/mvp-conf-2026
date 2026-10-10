using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.ListTasks;

public interface IListTasksPort
{
    Task<IReadOnlyList<TodoItem>> ListAsync();
}
