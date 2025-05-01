namespace Domain.GeminiTest;

public interface IGenerativeAiService
{
    public Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
}