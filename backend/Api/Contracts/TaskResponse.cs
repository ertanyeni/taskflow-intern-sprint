namespace TaskFlow.Api.Contracts;

/// <summary>
/// API dışarıya bu tipi döner. Domain entity'si (TaskItem) doğrudan dışarı sızmaz;
/// böylece iç model değişince API sözleşmesi kırılmaz.
/// </summary>
public record TaskResponse(
    Guid Id,
    string Title,
    string Status,
    DateTimeOffset CreatedAt);
