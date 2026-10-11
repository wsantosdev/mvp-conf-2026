using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CreateTask;

public interface ICreateTaskDatabasePort
{
    Task SaveAsync(TodoItem item);
}
