namespace HttpServer.Shared;

public class SettingsModel
{
    public string? StaticDirectoryPath { get; init; }
    public string? Domain { get; init; }
    public string? Port { get; init; }
}