using TaskFlow.Application.Tasks;

namespace TaskFlow.Api.Endpoints;

/// <summary>
/// Task HTTP route'ları. Endpoint sadece HTTP ile ilgilenir:
/// isteği alır, application service'i çağırır, HTTP response döner. İş kuralı barındırmaz.
/// </summary>
public static class TasksEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        // GET /api/tasks — çalışan iskeletin uçtan uca kanıtı: HTTP -> service -> EF Core -> PostgreSQL.
        group.MapGet("", async (ITaskService service, CancellationToken ct) =>
        {
            var tasks = await service.GetTasksAsync(ct);
            return Results.Ok(tasks);
        });

        // ---------------------------------------------------------------------
        // Aşağıdakiler BİLEREK boş bırakıldı — bunlar öğrenme modülleri:
        //
        //   POST   /api/tasks               -> Gün 2: CreateTask (AI referans modülü)
        //   PATCH  /api/tasks/{id}/status   -> Gün 3: UpdateTaskStatus (stajyer ayna modülü)
        //   DELETE /api/tasks/{id}          -> stajyer alıştırması
        //
        // AI_SENIOR_PARTNER_REHBERI.md ve backend/README.md'deki reference -> mirror akışı.
        // ---------------------------------------------------------------------

        return app;
    }
}
