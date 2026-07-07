namespace TaskFlow.Application.Ai;

/// <summary>
/// AI (LLM) sağlayıcısının soyutlaması. Somut implementasyon Groq'tur ama arayüz
/// sağlayıcıdan bağımsız. Testlerde FakeAiService ile değiştirilir (Groq'a çıkılmaz).
/// </summary>
public interface IAiService
{
    /// <summary>
    /// Verilen içeriği özetler ve etiketler üretir.
    /// </summary>
    /// <exception cref="AiNotConfiguredException">API key yapılandırılmadıysa.</exception>
    Task<AiResult> SummarizeAndTagAsync(string subject, string content, CancellationToken ct);
}

public record AiResult(string Summary, IReadOnlyList<string> Tags, string Model);

/// <summary>AI sağlayıcısı yapılandırılmadığında (key yok) fırlatılır → endpoint 503'e çevirir.</summary>
public sealed class AiNotConfiguredException : Exception
{
    public AiNotConfiguredException(string message) : base(message) { }
}

/// <summary>AI sağlayıcısına ulaşılamadığında / hata döndüğünde → endpoint 502'ye çevirir.</summary>
public sealed class AiProviderException : Exception
{
    public AiProviderException(string message, Exception? inner = null) : base(message, inner) { }
}
