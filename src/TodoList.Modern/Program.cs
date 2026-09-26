using TodoList.Modern.Features.CancelTask;
using TodoList.Modern.Features.CompleteTask;
using TodoList.Modern.Features.CreateTask;
using TodoList.Modern.Features.ListTasks;
using TodoList.Modern.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var memoryAdapter = new TodoStorageAdapter();
builder.Services.AddSingleton<ICreateTaskPort>(memoryAdapter);
builder.Services.AddSingleton<ICancelTaskPort>(memoryAdapter);
builder.Services.AddSingleton<ICompleteTaskPort>(memoryAdapter);
builder.Services.AddSingleton<IListTasksPort>(memoryAdapter);

builder.Services.AddTransient<CreateTaskHandler>();
builder.Services.AddTransient<CancelTaskHandler>();
builder.Services.AddTransient<CompleteTaskHandler>();
builder.Services.AddTransient<ListTasksHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "TodoList Modern API v1"));
}

app.MapCreateTaskEndpoint();
app.MapCancelTaskEndpoint();
app.MapCompleteTaskEndpoint();
app.MapListTasksEndpoint();

app.UseHttpsRedirection();

app.Run();

public partial class Program
{
}
