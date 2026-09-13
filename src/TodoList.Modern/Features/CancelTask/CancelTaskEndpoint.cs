using Microsoft.AspNetCore.Mvc;

namespace TodoList.Modern.Features.CancelTask;

public static class CancelTaskEndpoint
{
    public static IEndpointRouteBuilder MapCancelTaskEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/todos/{id:guid}/cancel", async (
                Guid id,
                [FromServices] CancelTaskHandler handler) =>
            {
                try
                {
                    await handler.HandleAsync(new CancelTaskCommand(id));
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
            .WithName("CancelTask");

        return app;
    }
}
