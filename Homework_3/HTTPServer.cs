using System.Net;
using HttpServer.Shared;

namespace HttpServerApp;

public sealed class HttpServer
{
    private static volatile HttpServer? _instance;
    private static readonly object Lock = new();

    private static readonly Dictionary<string, string> FileTypes = new()
    {
        { "html", "text/html" },
        { "css", "text/css" },
        { "js", "application/javascript" },
        { "png", "image/png" },
        { "jpeg", "image/jpeg" },
        { "jpg", "image/jpeg" },
        { "webp", "image/webp" },
        {"svg", "image/svg+xml" },
        {"ico", "image/x-icon" },
        
    };

    private readonly HttpListener _listener;
    private readonly SettingsModel _settings;

    private HttpServer(SettingsModel settings)
    {
        _settings = settings;
        _listener = new HttpListener();
    }

    public static HttpServer GetInstance(SettingsModel settings)
    {
        if (_instance is null)
            lock (Lock)
            {
                if (_instance is null)
                    _instance = new HttpServer(settings);
            }

        return _instance;
    }

    public void Start()
    {
        var prefix = $"{_settings.Domain}:{_settings.Port}/";
        _listener.Prefixes.Add(prefix);
        _listener.Start();
        Console.WriteLine($"{prefix}");
        Console.WriteLine("Сервер ожидает...");
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Сервер остановлен...");
    }

    private void Receive()
    {
        _listener.BeginGetContext(ListenerCallback, _listener);
    }

    private async void ListenerCallback(IAsyncResult result)
    {
        if (!_listener.IsListening) return;
        Receive();
        var context = _listener.EndGetContext(result);
        var request = context.Request;
        var response = context.Response;
        var path = _settings.StaticDirectoryPath + request.Url?.LocalPath;
        if (path[^1] == '/') path += "index.html";

        try
        {
            var responseText = await File.ReadAllBytesAsync(path);
            FileTypes.TryGetValue(path.Split(".")[1], out var type);
            response.ContentType = type;
            response.ContentLength64 = responseText.Length;
            await using var output = response.OutputStream;
            await output.WriteAsync(responseText);
            await output.FlushAsync();
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("static folder not found");
            response.StatusCode = 404;
            await using var output = response.OutputStream;
            await output.FlushAsync();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("index.html is not found in " + path);
            response.StatusCode = 404;
            await using var output = response.OutputStream;
            await output.FlushAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("There is an exception: " + e.Message);
            Stop();
        }

        Receive();
    }
}