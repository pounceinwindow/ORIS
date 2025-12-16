using HttpServer.Core.Attributes;

namespace MiniHttpServer.Controllers;

[Endpoint]
public class BonxController
{
    [HttpGet("/bonx")]
    public string LoginPage()
    {
        return "bonx/about.html";
    }
}