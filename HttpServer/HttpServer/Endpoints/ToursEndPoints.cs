using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;

namespace HttpServer.Endpoints;

[Endpoint]
public class ToursEndpoint : BaseEndpoint
{
    [HttpGet("/tours")]
    public string List()
    {
        return "/sem/index.html";
    }
}