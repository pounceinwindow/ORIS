using System.Net;
using CustomHttpServer.Core.Handlers;
using HttpServer.Shared;
using MiniHttpServer.Core.Handlers;
using ContentType = HttpServer.Shared.ContentType;

namespace HttpServerApp;

public sealed class HttpServer
{
    private SettingsModel _settings = SettingsModelSingleton.Instance;
    private HttpListener _listener = new();

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
        try
        {
            _listener.BeginGetContext(ListenerCallback, _listener);
        }
        catch (ObjectDisposedException)
        {
        }
        catch (HttpListenerException)
        {
        }
    }
 
    private async void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            Handler staticFilesHandler = new StaticFilesHandler(_settings.StaticDirectoryPath);
            Handler endpointsHandler = new EndpointsHandler();
            staticFilesHandler.Successor = endpointsHandler;
            staticFilesHandler.HandleRequest(context);

            if (_listener.IsListening)
                Receive();
        }
    }
}