using System.Text.Json;
using HttpServer.Shared;
try
{
    if (!File.Exists("settings.json"))
        throw new FileNotFoundException("Файл настроек не найден", "settings.json");
    string settingsJson = File.ReadAllText("settings.json");
    SettingsModel settings = JsonSerializer.Deserialize<SettingsModel>(settingsJson)
                             ?? throw new JsonException("settings.json incorrect");
    
    var server = HttpServerApp.HttpServer.GetInstance(settings);
    server.Start();
    Console.WriteLine("нажмите /stop для завершения.\n");

    while (true)
    {
        
        var stop = Console.ReadLine();
        if (stop?.Trim().ToLower() == "/stop")
        {
            server.Stop(); 
            break; 
        }
    }
}
catch (Exception e)
{
    Console.WriteLine("Ошибка: " + e.Message);
}