using System.Text.Json;
using HttpServer.Shared;
try
{
    string settingsJson = File.ReadAllText("settings.json");
    SettingsModel settings = JsonSerializer.Deserialize<SettingsModel>(settingsJson)
                             ?? throw new JsonException("settings.json incorrect");
    
    var server = new HttpServerApp.HttpServer(settings);
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