using TodoList.Layered.Endpoints;
using TodoList.Layered.Infrastructure.Database;
using TodoList.Layered.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "TodoList Layered API v1"));
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();

public partial class Program
{
}