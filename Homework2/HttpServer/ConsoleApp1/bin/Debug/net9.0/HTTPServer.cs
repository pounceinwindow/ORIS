using System.Net;
using System.Text;
using HttpServer.Shared;

namespace HttpServerApp;

public class HttpServer
{
    private readonly HttpListener _listener;
    private readonly SettingsModel _settings;

    public HttpServer(SettingsModel settings)
    {
        _settings = settings;
        _listener = new HttpListener();
    }

    public void Start()
    {
        var prefix = $"{_settings.Domain}:{_settings.Port}/";
        _listener.Prefixes.Add(prefix);
        _listener.Start();
        Console.WriteLine($"{prefix}");
        Console.WriteLine("Сервер ожидает...");
        Task.Run(ListenLoop);
    }

    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Сервер остановлен...");
    }


    private async Task ListenLoop()
    {
        while (_listener.IsListening)
            try
            {
                var context = await _listener.GetContextAsync();
                var response = context.Response;
                var isCss =
                    context.Request.Url?.AbsolutePath.EndsWith("/gpt.css", StringComparison.OrdinalIgnoreCase) == true;
                var file = isCss ? "gpt.css" : "chatgpt.html";

                var responseText = File.ReadAllText($"{_settings.StaticDirectoryPath}{file}");
                var buffer = Encoding.UTF8.GetBytes(responseText);

                response.ContentType = isCss ? "text/css; charset=utf-8" : "text/html; charset=utf-8";
                response.ContentLength64 = buffer.Length;

                await using var output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();
                Console.WriteLine("Запрос обработан");
                
            }
            
            catch (HttpListenerException)
            {
                break;
            }
    }
}