namespace TaskFlow.Application.Ai;

/// <summary>
/// Groq yapılandırması. appsettings.json'daki "Groq" bölümünden bağlanır (Program.cs).
/// GERÇEK KEY appsettings'te DEĞİL: dotnet user-secrets veya Groq__ApiKey env değişkeni.
/// </summary>
public sealed class GroqOptions
{
    public const string SectionName = "Groq";

    public string ApiKey { get; set; } = string.Empty;

    public string Model { get; set; } = "llama-3.1-8b-instant";

    public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1/";
}
