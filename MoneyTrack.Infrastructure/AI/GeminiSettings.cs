namespace MoneyTrack.Infrastructure.AI;

public class GeminiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
    public string Model { get; set; } = string.Empty;
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.1;
    public int TimeoutSeconds { get; set; } = 30;

    override 
    public string ToString()
    {
        return $"{BaseUrl}/models/{Model}:generateContent?key={ApiKey}";
    }
}