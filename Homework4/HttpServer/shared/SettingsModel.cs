using System.Text.Json;

namespace HttpServer.Shared;

public class SettingsModel
{
    public string? StaticDirectoryPath { get; init; }
    public string? Domain { get; init; }
    public string? Port { get; init; }

    public static SettingsModel ReadJSON(string path) =>
        JsonSerializer.Deserialize<SettingsModel>(File.ReadAllText(path));
}