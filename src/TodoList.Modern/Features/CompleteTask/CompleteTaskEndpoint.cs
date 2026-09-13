using Microsoft.AspNetCore.Mvc;

namespace TodoList.Modern.Features.CompleteTask;

public static class CompleteTaskEndpoint
{
    public static IEndpointRouteBuilder MapCompleteTaskEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/todos/{id:guid}/complete", async (
                Guid id,
                [FromServices] CompleteTaskHandler handler) =>
            {
                try
                {
                    await handler.HandleAsync(new CompleteTaskCommand(id));
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { Error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            })
            .WithTags("Todos - VSA")
            .WithName("CompleteTask");

        return app;
    }
}
