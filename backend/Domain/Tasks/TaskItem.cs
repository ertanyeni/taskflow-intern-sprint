namespace TaskFlow.Domain.Tasks;

/// <summary>
/// Task domain entity'si. İsmi "Task" değil çünkü .NET'in System.Threading.Tasks.Task
/// tipiyle çakışır. İş kuralları burada korunur.
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

    public DateTimeOffset CreatedAt { get; set; }
}
