using Microsoft.AspNetCore.Mvc;
using TodoList.NTier.Service;

namespace TodoList.NTier.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/todos").WithTags("Todos - N-Tier");
        group.MapPost("/", async ([FromBody] CreateTodoDto dto, [FromServices] ITodoService service) =>
        {
            try
            {
                var id = await service.CreateAsync(dto);
                return Results.Created($"/api/v1/todos/{id}", new { Id = id, Message = "Tarefa criada com sucesso (N-Tier)!" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });
        group.MapPost("/{id:guid}/cancel", async (Guid id, [FromServices] ITodoService service) =>
        {
            try
            {
                await service.CancelAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex) { return Results.NotFound(new { Error = ex.Message }); }
            catch (InvalidOperationException ex) { return Results.BadRequest(new { Error = ex.Message }); }
        });
        group.MapPost("/{id:guid}/complete", async (Guid id, [FromServices] ITodoService service) =>
        {
            try
            {
                await service.CompleteAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex) { return Results.NotFound(new { Error = ex.Message }); }
            catch (InvalidOperationException ex) { return Results.BadRequest(new { Error = ex.Message }); }
        });
        return app;
    }
}
