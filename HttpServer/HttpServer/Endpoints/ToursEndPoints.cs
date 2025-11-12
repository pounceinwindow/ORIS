using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using HttpServer.Framework.core.Attributes;
using HttpServer.Framework.Core.HttpResponse;
using HttpServer.Framework.Settings;
using MyORM;

namespace HttpServer.Endpoints;

[Endpoint]
public class ToursEndpoint : BaseEndpoint
{
    [HttpGet("/tours")]
public IResponseResult List()
{
    var qs = Context.Request.QueryString;

    var city     = qs["city"];
    var text     = qs["q"];
    var minPrice = TryParseDecimal(qs["minPrice"]);
    var maxPrice = TryParseDecimal(qs["maxPrice"]);
    var ratingMin= TryParseDecimal(qs["ratingMin"]);
    var instant  = ParseBool(qs["instant"]);
    var free     = ParseBool(qs["free"]);
    var categories = qs.GetValues("category")?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList()
                    ?? new List<string>();

    var orm = new OrmContext(SettingsManager.Instance.Settings.ConnectionString!);

    Expression<Func<Experience, bool>> predicate =
        BuildPredicate(city, text, minPrice, maxPrice, ratingMin, instant, free, categories);

    var items = IsNoFilter(city, text, minPrice, maxPrice, ratingMin, instant, free, categories)
        ? orm.ReadAll<Experience>("experiences").ToList()
        : orm.Where(predicate, "experiences").ToList();

    // 5 «канонических» категорий как на musement
    var canonical = new[]
    {
        "Attractions & guided tours",
        "Excursions & day trips",
        "Activities",
        "Experiences for locals",
        "Tickets & events"
    };

    // чекбоксы слева — union фактических и канонических (чтобы всегда показать 5)
    var dataCats = orm.ReadAll<Experience>("experiences")
        .Select(e => e.CategoryName?.Trim())
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Distinct(StringComparer.OrdinalIgnoreCase);

    var availableCategories = canonical
        .Union(dataCats, StringComparer.OrdinalIgnoreCase)
        .ToList();

    var vm = new IndexViewModel
    {
        City = string.IsNullOrWhiteSpace(city)
            ? "Tickets, activities and visits in Tours"
            : $"Tickets, activities and visits in {city}",
        Total = items.Count,
        Items = items,
        AvailableCategories = availableCategories,

        // echo
        CityFilter     = city ?? "",
        Q              = text ?? "",
        MinPriceText   = minPrice?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
        MaxPriceText   = maxPrice?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
        RatingMinText  = ratingMin?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
        Instant        = instant == true,
        Free           = free == true,
        SelectedCategories = categories,

        // верхние табы — ровно 5
        TopCategories  = canonical.ToList(),
        ActiveCategory = (categories.Count == 1 ? categories[0] : "")
    };

    return Page("sem/index.html", vm);
}


    private static Expression<Func<Experience, bool>> BuildPredicate(
        string? city, string? text, decimal? minPrice, decimal? maxPrice, decimal? ratingMin,
        bool? instant, bool? free, List<string> categories)
    {
        var p = Expression.Parameter(typeof(Experience), "e");
        Expression body = Expression.Constant(true);

        if (!string.IsNullOrWhiteSpace(city))
            body = Expression.AndAlso(body,
                Expression.Equal(Expression.Property(p, nameof(Experience.City)), Expression.Constant(city)));

        if (!string.IsNullOrWhiteSpace(text))
        {
            var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            var tCond = Expression.Call(Expression.Property(p, nameof(Experience.Title)), contains, Expression.Constant(text));
            var dCond = Expression.Call(Expression.Property(p, nameof(Experience.Description)), contains, Expression.Constant(text));
            body = Expression.AndAlso(body, Expression.OrElse(tCond, dCond));
        }

        if (minPrice.HasValue)
            body = Expression.AndAlso(body,
                Expression.GreaterThanOrEqual(Expression.Property(p, nameof(Experience.PriceFrom)),
                    Expression.Constant(minPrice.Value)));

        if (maxPrice.HasValue)
            body = Expression.AndAlso(body,
                Expression.LessThanOrEqual(Expression.Property(p, nameof(Experience.PriceFrom)),
                    Expression.Constant(maxPrice.Value)));

        if (ratingMin.HasValue)
            body = Expression.AndAlso(body,
                Expression.GreaterThanOrEqual(Expression.Property(p, nameof(Experience.Rating)),
                    Expression.Constant(ratingMin.Value)));

        if (instant == true)
            body = Expression.AndAlso(body,
                Expression.Equal(Expression.Property(p, nameof(Experience.InstantConfirmation)), Expression.Constant(true)));

        if (free == true)
            body = Expression.AndAlso(body,
                Expression.Equal(Expression.Property(p, nameof(Experience.FreeCancellation)), Expression.Constant(true)));

        if (categories != null && categories.Count > 0)
        {
            var contains = typeof(Enumerable).GetMethods()
                .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(string));
            var call = Expression.Call(contains, Expression.Constant(categories),
                Expression.Property(p, nameof(Experience.CategoryName)));
            body = Expression.AndAlso(body, call);
        }

        return Expression.Lambda<Func<Experience, bool>>(body, p);
    }

    private static bool IsNoFilter(string? city, string? text, decimal? minPrice, decimal? maxPrice, decimal? ratingMin,
        bool? instant, bool? free, List<string> categories)
        => string.IsNullOrWhiteSpace(city)
           && string.IsNullOrWhiteSpace(text)
           && !minPrice.HasValue && !maxPrice.HasValue && !ratingMin.HasValue
           && instant != true && free != true
           && (categories == null || categories.Count == 0);

    private static decimal? TryParseDecimal(string? s)
        => decimal.TryParse(s, System.Globalization.NumberStyles.Any,
               System.Globalization.CultureInfo.InvariantCulture, out var d)
           ? d : null;

    private static bool? ParseBool(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
        if (s == "0" || s.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
        return null;
    }
}
