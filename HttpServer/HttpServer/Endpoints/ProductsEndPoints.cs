using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;
using HttpServer.Framework.Settings;
using MyORM;

[Endpoint]
public class ProductEndpoint : BaseEndpoint
{
    [HttpGet("/product")]
    public IResponseResult Detail()
    {
        var slug = Context.Request.QueryString["slug"];
        if (string.IsNullOrWhiteSpace(slug))
            return Page("sem/404.html", null);

        var settings = SettingsManager.Instance.Settings;
        var db = new OrmContext(settings.ConnectionString);

        var exp = db.FirstOrDefault<Experience>(
            e => e.Slug == slug,
            "experiences"
        );
        if (exp == null)
            return Page("sem/404.html", null);

        var details = db.FirstOrDefault<ExperienceDetails>(
            d => d.ExperienceId == exp.Id,
            "experience_details"
        );

        var reviews = db.Where<Review>(
            r => r.ExperienceId == exp.Id,
            "reviews"
        ).ToList();

        var related = db.Where<Experience>(
            e => e.City == exp.City && e.Id != exp.Id,
            "experiences"
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