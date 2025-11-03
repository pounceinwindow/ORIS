using HttpServer.Framework.core.Attributes;

namespace HttpServer.Endpoints;

[Endpoint]
public class BonxEndpoing
{
    [HttpGet("/bonx")]
    public string LoginPage()
    {
        return "bonx/about.html";
    }
}