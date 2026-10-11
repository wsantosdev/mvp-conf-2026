using TodoList.Modern.Domain;
using TodoList.Modern.Infrastructure;

namespace TodoList.Modern.Features.CreateTask;

public sealed class CreateTaskDatabaseAdapter(InMemoryDatabase database) : ICreateTaskDatabasePort
{
    public Task SaveAsync(TodoItem item)
    {
        database.Save(item);
        return Task.CompletedTask;
    }
}
