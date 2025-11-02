using System.Net;
using System.Text;
using System.Text.Json;
using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Settings;
using MyORMLibrary;
using Npgsql;

namespace HttpServer.Endpoints;

[Endpoint]
internal class UserEndpoint
{
    [HttpGet("/users")]
    public async Task GetUsers(HttpListenerContext ctx)
    {
        try
        {
            var orm = new ORMContext(SettingsManager.Instance.Settings.ConnectionString!);
            if (int.TryParse(ctx.Request.QueryString["id"], out var id))
            {
                var user = orm.ReadById<UserModel>(id, "users");
                if (user == null)
                {
                    await Write(ctx, "{\"error\":\"not_found\"}", 404, "application/json; charset=utf-8");
                    return;
                }
                await Write(ctx, JsonSerializer.Serialize(user), 200, "application/json; charset=utf-8");
                return;
            }

            var list = orm.ReadAll<UserModel>("users");
            await Write(ctx, JsonSerializer.Serialize(list), 200, "application/json; charset=utf-8");
        }
        catch
        {
            await Write(ctx, "{\"error\":\"server_error\"}", 500, "application/json; charset=utf-8");
        }
    }

    private static async Task Write(HttpListenerContext c, string s, int status, string ct)
    {
        var b = Encoding.UTF8.GetBytes(s);
        c.Response.StatusCode = status;
        c.Response.ContentType = ct;
        c.Response.ContentLength64 = b.Length;
        await using var o = c.Response.OutputStream;
        await o.WriteAsync(b, 0, b.Length);
        await o.FlushAsync();
    }
}