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
        var repo = new LinqExpressionToSql();

        var data = new
        {
            Users = new
            {
                // All = repo.Where<UserModel>(x => x.Name.Contains("sosi")).ToList()
                All = orm.ReadAll<UserModel>("users")
            }
            
        };

        return Page("/index.html", data);
    }
}