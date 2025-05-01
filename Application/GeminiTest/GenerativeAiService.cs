using System.Net.Http.Json;
using System.Text.Json;
using Domain.GeminiTest;
using Microsoft.Extensions.Configuration;

namespace Application.GeminiTest;

public class GenerativeAiService(
    HttpClient httpClient,
    IConfiguration configuration
    ) : IGenerativeAiService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = configuration["Gemini:ApiKey"]!;

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        var req = new
        {
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } }
        };

        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={this._apiKey}")
        {
            Content = JsonContent.Create(req)
        };
        message.Headers.Add("x-goog-api-key", $"{this._apiKey}");
        
        var resp = await _httpClient.SendAsync(message, ct);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return json
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString()!;
    }
}