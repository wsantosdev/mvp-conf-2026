using Microsoft.AspNetCore.Mvc;

namespace TodoList.Modern.Features.ListTasks;

public static class ListTasksEndpoint
{
    public static IEndpointRouteBuilder MapListTasksEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/todos", async ([FromServices] ListTasksHandler handler) =>
                Results.Ok(await handler.HandleAsync()))
            .WithTags("Todos - VSA")
            .WithName("ListTasks");

        return app;
    }
}
