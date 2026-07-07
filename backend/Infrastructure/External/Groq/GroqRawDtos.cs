using System.Text.Json.Serialization;

namespace TaskFlow.Infrastructure.External.Groq;

// Groq, OpenAI-uyumlu "chat/completions" sözleşmesini kullanır. Bu ham şekiller
// sadece Infrastructure içinde yaşar. Aynı şekil Groq/OpenRouter/HF için de geçerli.

internal sealed class ChatCompletionRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = [];

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.3;

    [JsonPropertyName("response_format")]
    public ResponseFormat ResponseFormat { get; set; } = new();
}

internal sealed class ResponseFormat
{
    // JSON modu: modelin çıktısını geçerli JSON'a zorlar.
    [JsonPropertyName("type")]
    public string Type { get; set; } = "json_object";
}

internal sealed class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

internal sealed class ChatCompletionResponse
{
    [JsonPropertyName("choices")]
    public List<ChatChoice>? Choices { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }
}

internal sealed class ChatChoice
{
    [JsonPropertyName("message")]
    public ChatMessage? Message { get; set; }
}

// Modelden istediğimiz JSON şekli: { "summary": "...", "tags": ["...", "..."] }
internal sealed class AiJsonPayload
{
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
}
