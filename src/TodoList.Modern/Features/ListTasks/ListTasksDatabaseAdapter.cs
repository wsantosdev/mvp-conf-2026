using TodoList.Modern.Domain;
using TodoList.Modern.Infrastructure;

namespace TodoList.Modern.Features.ListTasks;

public sealed class ListTasksDatabaseAdapter(InMemoryDatabase database) : IListTasksDatabasePort
{
    public Task<IReadOnlyList<TodoItem>> ListAsync() => Task.FromResult(database.GetAll());
}
