using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;
using HttpServer.Framework.Settings;
using MyORM;

namespace HttpServer.Endpoints;

[Endpoint]
public class ProductEndpoint : BaseEndpoint
{
    [HttpGet("/product")]
    public IResponseResult Detail()
    {
        var qs = Context.Request.QueryString;
        var slug = qs["slug"];
        var idStr = qs["id"];

        var settings = SettingsManager.Instance.Settings;
        var db = new OrmContext(settings.ConnectionString);

        // 1) Находим сам experience по slug или id
        Experience? exp = null;
        if (!string.IsNullOrWhiteSpace(slug))
            exp = db.FirstOrDefault<Experience>(e => e.Slug == slug, "experiences");
        else if (int.TryParse(idStr, out var pid))
            exp = db.FirstOrDefault<Experience>(e => e.Id == pid, "experiences");
        else
            exp = db.ReadAll<Experience>("experiences").FirstOrDefault();

        if (exp == null)
            // Можно вывести простую страницу 404
            return Page("sem/product_not_found.html", new { slug, id = idStr });

        // 2) Детали турпродукта
        var details = db.FirstOrDefault<ExperienceDetails>(
            d => d.ExperienceId == exp.Id, "experience_details"
        ) ?? new ExperienceDetails
        {
            ExperienceId = exp.Id,
            Title = exp.Title,
            City = exp.City,
            Category = exp.CategoryName,
            Price = exp.PriceFrom,
            Rating = exp.Rating,
            Reviews = exp.ReviewsCount,
            Hero = string.IsNullOrEmpty(exp.HeroUrl) ? "/sem/img/1.avif" : exp.HeroUrl
        };

        // 3) Отзывы
        var reviews = db.Where<Review>(r => r.ExperienceId == exp.Id, "reviews").ToList();

        // 4) Похожие туры (по городу), можно ограничить 4
        var related = db.Where<Experience>(
            e => e.City == exp.City && e.Id != exp.Id, "experiences"
        ).Take(4).ToList();

        var vm = new ProductViewModel
        {
            Experience = exp,
            Details = details,
            Reviews = reviews,
            RelatedTours = related
        };

        return Page("sem/product.html", vm);
    }
}