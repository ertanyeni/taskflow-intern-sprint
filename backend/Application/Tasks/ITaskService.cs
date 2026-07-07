using TaskFlow.Api.Contracts;

namespace TaskFlow.Application.Tasks;

/// <summary>
/// Task use-case'lerinin sözleşmesi. Endpoint bu arayüzle konuşur, somut sınıfla değil.
/// </summary>
public interface ITaskService
{
    Task<IReadOnlyList<TaskResponse>> GetTasksAsync(CancellationToken ct);

    // Gün 2+ eklenecek use-case'ler (referans / ayna modülleri):
    // Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct);
    // Task<TaskResponse?> UpdateStatusAsync(Guid id, UpdateTaskStatusRequest request, CancellationToken ct);
    // Task<bool> DeleteTaskAsync(Guid id, CancellationToken ct);
}
