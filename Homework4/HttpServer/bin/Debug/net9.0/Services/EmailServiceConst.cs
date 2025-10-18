using System.Text.Json;

namespace HttpServer.Services;

public class EmailServiceConst
{
    public required string SmtpHost  { get; init; }
    public required int    SmtpPort  { get; init; }
    public required string SmtpUser  { get; init; }
    public required string SmtpPass  { get; init; }
    public required string FromAddr  { get; init; }
    public required string FromName  { get; init; }
    public static EmailServiceConst LoadConfig()
    {
        var json = File.ReadAllText("settings.json");
        return JsonSerializer.Deserialize<EmailServiceConst>(json)
               ?? throw new InvalidOperationException($"Bad email  ServiceConst");
    }
}