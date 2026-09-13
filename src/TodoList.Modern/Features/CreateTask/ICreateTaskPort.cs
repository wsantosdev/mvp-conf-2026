using TodoList.Modern.Domain;

namespace TodoList.Modern.Features.CreateTask;

public interface ICreateTaskPort
{
    Task SaveAsync(TodoItem item);
}
