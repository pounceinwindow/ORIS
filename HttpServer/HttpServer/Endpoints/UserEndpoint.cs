using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;
using HttpServer.Framework.Settings;
using MyORM;

namespace HttpServer.Endpoints;

[Endpoint]
internal class UserEndpoint : BaseEndpoint
{
    [HttpGet("/users")]
    public IResponseResult GetUsers()
    {
        var orm = new OrmContext(SettingsManager.Instance.Settings.ConnectionString!);

        var data = new
        {
            Users = new
            {
                All = orm.ReadAll<UserModel>("users").Where(x => x.Name.Contains("Putin"))
            }
        };

        return Page("/index.html", data);
    }
}