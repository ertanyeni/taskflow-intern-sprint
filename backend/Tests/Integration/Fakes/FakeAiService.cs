using TaskFlow.Application.Ai;

namespace TaskFlow.Tests.Integration.Fakes;

/// <summary>
/// IAiService test sahtesi. Groq'a ÇIKMAZ; sabit özet/etiket döner.
/// CI'da gerçek Groq key GEREKMEZ.
/// </summary>
public sealed class FakeAiService : IAiService
{
    public Task<AiResult> SummarizeAndTagAsync(string subject, string content, CancellationToken ct)
        => Task.FromResult(new AiResult(
            Summary: $"{subject} için test özeti.",
            Tags: ["test", "fake"],
            Model: "fake-model"));
}
