using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Contracts;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Application.Tasks;

/// <summary>
/// Task use-case'lerini yürütür. EF Core ile konuşur, entity'yi DTO'ya çevirip döner.
/// </summary>
public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TaskResponse>> GetTasksAsync(CancellationToken ct)
    {
        return await _db.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TaskResponse(
                t.Id,
                t.Title,
                t.Status.ToString(),
                t.CreatedAt))
            .ToListAsync(ct);
    }
}
