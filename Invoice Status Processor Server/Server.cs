using System.Net;
using System.Text;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;

class Config
{
    public int ProcessingIntervalSeconds { get; set; }
    public int MaxErrorRetries { get; set; }
    public string ConnectionString { get; set; }
}

class Program
{
    
    static Config config;
    static DateTime lastConfigRead;
    static Random rnd = new Random();

    static int lastPending, lastSuccess, lastError;

    static async Task Main()
    {
        LoadConfig();

        var server = new HttpListener();
        server.Prefixes.Add("http://localhost:8080/");
        server.Start();
        Console.WriteLine("Server started");

        var q = Task.Run(() => HttpLoop(server));

        while (true)
        {
            LoadConfigIfChanged();
            await Task.Delay(config.ProcessingIntervalSeconds * 1000);
        }
    }


    static void LoadConfig()
    {
        var json = File.ReadAllText("config.json");
        config = JsonSerializer.Deserialize<Config>(json)!;
        lastConfigRead = File.GetLastWriteTime("config.json");
        Console.WriteLine("Config loaded");
    }

    static void LoadConfigIfChanged()
    {
        var t = File.GetLastWriteTime("config.json");
        if (t > lastConfigRead)
            LoadConfig();
    }


    static async Task HttpLoop(HttpListener server)
    {
        while (true)
        {
            var ctx = await server.GetContextAsync();
            var path = ctx.Request.Url!.AbsolutePath;

            if (path == "/health")
                Reply(ctx, "OK");

            else if (path == "/config")
                Reply(ctx, JsonSerializer.Serialize(config));

            else if (path == "/config/reload")
            {
                LoadConfig();
                Reply(ctx, "reloaded");
            }
            else if (path == "/stats")
            {
                Reply(ctx, JsonSerializer.Serialize(new
                {
                    lastPending,
                    lastSuccess,
                    lastError
                }));
            }
            else
                ctx.Response.StatusCode = 404;

            ctx.Response.Close();
        }
    }

    static void Reply(HttpListenerContext ctx, string text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        ctx.Response.ContentLength64 = data.Length;
        ctx.Response.OutputStream.Write(data);
    }


}