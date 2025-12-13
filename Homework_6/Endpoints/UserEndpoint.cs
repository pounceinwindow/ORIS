using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;
using HttpServer.Framework.Settings;
using MyORMLibrary;

namespace HttpServer.Endpoints;

[Endpoint]
internal class UserEndpoint : BaseEndpoint
{
    [HttpGet("/users")]
    public IResponseResult GetUsers()
    {
        var orm = new ORMContext(SettingsManager.Instance.Settings.ConnectionString!);

        var data = new
        {
            Users = new
            {
                Items = orm.ReadAll<UserModel>("users")
            }
        };

        return Page("/index.html", data);
    }
}