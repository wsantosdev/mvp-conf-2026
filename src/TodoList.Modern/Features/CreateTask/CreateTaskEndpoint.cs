using Microsoft.AspNetCore.Mvc;

namespace TodoList.Modern.Features.CreateTask;

public static class CreateTaskEndpoint
{
    public static IEndpointRouteBuilder MapCreateTaskEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/todos", async (
                [FromBody] CreateTaskCommand command,
                [FromServices] CreateTaskHandler handler) =>
            {
                try
                {
                    var id = await handler.HandleAsync(command);
                    return Results.Created($"/api/v1/todos/{id}", new { Id = id, Message = "Tarefa criada com sucesso!" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            })
            .WithTags("Todos - VSA")
            .WithName("CreateTask");

        return app;
    }
}
