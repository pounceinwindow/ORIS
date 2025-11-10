// using HttpServer.Framework.core.Attributes;
// using HttpServer.Framework.Settings;
//
// namespace HttpServer.Endpoints;
//
// [Endpoint]
// public class AdminEndpoint
// {
//     [HttpGet("/admin/tours")]
//     public string AdminTours()
//     {
//         // TODO: Добавить авторизацию
//         var tours = _db.ReadAll<Tour>();
//         var template = File.ReadAllText("Templates/admin-tours.template.html");
//         return RenderTemplate(template, new { tours });
//     }
//     
//     [HttpPost("/admin/tours/create")]
//     public string CreateTour(/* параметры формы */)
//     {
//         // CRUD операции для туров
//     }
// }

