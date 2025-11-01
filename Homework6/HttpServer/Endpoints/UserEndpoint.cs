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
    public async Task GetUser(HttpListenerContext ctx)
    {
        try
        {
            var conn = SettingsManager.Instance.Settings.ConnectionString!;
            if (int.TryParse(ctx.Request.QueryString["id"], out var id))
            {
                var orm = new ORMContext(conn);
                var UserModel = orm.ReadById<UserModel>(id, "users");
                if (UserModel == null)
                {
                    await Write(ctx, "{\"error\":\"not_found\"}", 404, "application/json; charset=utf-8");
                    return;
                }

                await Write(ctx, JsonSerializer.Serialize(UserModel), 200, "application/json; charset=utf-8");
                return;
            }

            using var ds = NpgsqlDataSource.Create(conn);
            using var cmd = ds.CreateCommand("SELECT id, name, email FROM users");
            using var r = cmd.ExecuteReader();
            var list = new List<UserModel>();
            while (r.Read())
                list.Add(new UserModel
                {
                    Id = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                    Name = r.IsDBNull(1) ? null : r.GetString(1),
                    Email = r.IsDBNull(2) ? null : r.GetString(2)
                });
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