using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TaskFlow.Application.Ai;

namespace TaskFlow.Infrastructure.External.Groq;

/// <summary>
/// IAiService'in Groq (OpenAI-uyumlu) implementasyonu. Typed HttpClient + IOptions.
/// Key server-side kalır; frontend hiçbir zaman görmez (AI çağrısı hep backend'den geçer).
/// </summary>
public sealed class GroqAiService : IAiService
{
    private readonly HttpClient _http;
    private readonly GroqOptions _options;

    public GroqAiService(HttpClient http, IOptions<GroqOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<AiResult> SummarizeAndTagAsync(string subject, string content, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            // Key yoksa çökme yok: anlamlı bir domain hatası; endpoint bunu 503'e çevirir.
            throw new AiNotConfiguredException(
                "Groq API key yapılandırılmadı. 'dotnet user-secrets set \"Groq:ApiKey\" \"gsk_...\"' çalıştır.");
        }

        var request = new ChatCompletionRequest
        {
            Model = _options.Model,
            Messages =
            [
                new ChatMessage
                {
                    Role = "system",
                    Content = "Sen bir içerik asistanısın. SADECE şu şekilde JSON dön: "
                            + "{\"summary\": \"2-3 cümlelik Türkçe özet\", \"tags\": [\"3-5 kısa etiket\"]}."
                },
                new ChatMessage
                {
                    Role = "user",
                    Content = $"Konu: {subject}\n\nİçerik:\n{content}"
                }
            ]
        };

        HttpResponseMessage response;
        try
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
            {
                Content = JsonContent.Create(request)
            };
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            response = await _http.SendAsync(message, ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new AiProviderException("AI sağlayıcısına ulaşılamadı.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new AiProviderException($"AI sağlayıcısı {(int)response.StatusCode} döndü.");
        }

        var completion = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken: ct);
        var raw = completion?.Choices?.FirstOrDefault()?.Message?.Content;
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new AiProviderException("AI sağlayıcısı boş yanıt döndü.");
        }

        AiJsonPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<AiJsonPayload>(raw);
        }
        catch (JsonException ex)
        {
            throw new AiProviderException("AI yanıtı beklenen JSON formatında değil.", ex);
        }

        return new AiResult(
            payload?.Summary?.Trim() ?? string.Empty,
            payload?.Tags ?? [],
            completion?.Model ?? _options.Model);
    }
}
