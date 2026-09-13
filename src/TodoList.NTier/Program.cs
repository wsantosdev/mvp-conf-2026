using TodoList.NTier.Endpoints;
using TodoList.NTier.Infrastructure.Database;
using TodoList.NTier.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();

public partial class Program
{
}