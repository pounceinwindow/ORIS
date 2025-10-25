using System.Text.Json;

public class SettingsModel
{
    public string? StaticDirectoryPath { get; init; }
    public string? Domain { get; init; }
    public string? Port { get; init; }

    public static SettingsModel ReadJSON(string path)
    {
        return JsonSerializer.Deserialize<SettingsModel>(File.ReadAllText(path));
    }
}