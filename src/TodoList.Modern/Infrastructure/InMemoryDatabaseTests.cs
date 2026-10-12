using System.Threading.Tasks;
using TodoList.Modern.Domain;
using TodoList.Modern.Features.CancelTask;
using TodoList.Modern.Features.CompleteTask;
using TodoList.Modern.Features.CreateTask;
using TodoList.Modern.Features.ListTasks;
using TodoList.Modern.Infrastructure;
using Xunit;

namespace TodoList.Modern.Tests;

public class InMemoryDatabaseTests
{
    [Fact]
    public async Task Adapters_WithSharedDatabase_ExposeAndUpdateSameTodo()
    {
        var database = new InMemoryDatabase();
        var createAdapter = new CreateTaskDatabaseAdapter(database);
        var cancelAdapter = new CancelTaskDatabaseAdapter(database);
        var completeAdapter = new CompleteTaskDatabaseAdapter(database);
        var listAdapter = new ListTasksDatabaseAdapter(database);
        var todo = new TodoItem("Share between slices");

        await createAdapter.SaveAsync(todo);

        Assert.Same(todo, await cancelAdapter.GetByIdAsync(todo.Id));
        Assert.Same(todo, await completeAdapter.GetByIdAsync(todo.Id));
        Assert.Contains(todo, await listAdapter.ListAsync());

        todo.Cancel();
        await cancelAdapter.UpdateAsync(todo);

        var updatedTodo = await completeAdapter.GetByIdAsync(todo.Id);
        Assert.Same(todo, updatedTodo);
        Assert.True(updatedTodo!.IsCancelled);
        Assert.False(updatedTodo.IsCompleted);
    }

    [Fact]
    public async Task Adapters_WithUnknownId_ReturnNull()
    {
        var database = new InMemoryDatabase();
        var cancelAdapter = new CancelTaskDatabaseAdapter(database);
        var completeAdapter = new CompleteTaskDatabaseAdapter(database);
        var id = Guid.NewGuid();

        Assert.Null(await cancelAdapter.GetByIdAsync(id));
        Assert.Null(await completeAdapter.GetByIdAsync(id));
    }

}
