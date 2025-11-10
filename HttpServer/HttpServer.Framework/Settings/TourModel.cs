using System.Text.Json;

public class Experience
{
    public int Id { get; set; }
    public string Slug { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";
    public decimal PriceFrom { get; set; }
    public decimal Rating { get; set; }
    public int ReviewsCount { get; set; }
    public string CategoryName { get; set; } = "";
    public string HeroUrl { get; set; } = "";
    public bool InstantConfirmation { get; set; }
    public bool FreeCancellation { get; set; }
    public bool SkipTheLine { get; set; }
    public bool GuidedTour { get; set; }
    public bool EntranceFeesIncluded { get; set; }
    public bool PrivateTour { get; set; }
    public bool MealIncluded { get; set; }
}

public class ExperienceDetails
{
    public int Id { get; set; }
    public int ExperienceId { get; set; }
    public string Category { get; set; } = "";
    public string City { get; set; } = "";
    public string Title { get; set; } = "";
    public string Hero { get; set; } = "";
    public decimal Rating { get; set; }
    public string RatingText { get; set; } = "Excellent";
    public int Reviews { get; set; }
    public string Languages { get; set; } = "";
    public string Duration { get; set; } = "";
    public decimal Price { get; set; }
    public string Address { get; set; } = "";
    public string Meeting { get; set; } = "";
    public string CancelPolicy { get; set; } = "";
    public DateTime? ValidUntil { get; set; }

    // JSON поля для хранения списков
    public string ChipsJson { get; set; } = "[]";
    public string LoveJson { get; set; } = "[]";
    public string IncludedJson { get; set; } = "[]";
    public string RememberJson { get; set; } = "[]";
    public string MoreJson { get; set; } = "[]";

    // Вычисляемые свойства для удобства
    public List<string> Chips => JsonSerializer.Deserialize<List<string>>(ChipsJson ?? "[]") ?? new List<string>();
    public List<string> Love => JsonSerializer.Deserialize<List<string>>(LoveJson ?? "[]") ?? new List<string>();

    public List<string> Included =>
        JsonSerializer.Deserialize<List<string>>(IncludedJson ?? "[]") ?? new List<string>();

    public List<string> Remember =>
        JsonSerializer.Deserialize<List<string>>(RememberJson ?? "[]") ?? new List<string>();

    public List<MoreCard> More => JsonSerializer.Deserialize<List<MoreCard>>(MoreJson ?? "[]") ?? new List<MoreCard>();
}

public class MoreCard
{
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Img { get; set; } = "";
    public decimal Price { get; set; }
}

public class Review
{
    public int Id { get; set; }
    public int ExperienceId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = "";
    public string Author { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

// View Models
public class IndexViewModel
{
    public string City { get; set; } = "";
    public int Total { get; set; }
    public List<Experience> Items { get; set; } = new();
    public List<string> AvailableCategories { get; set; } = new();
}

public class ProductViewModel
{
    public Experience Experience { get; set; } = new();
    public ExperienceDetails Details { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public List<Experience> RelatedTours { get; set; } = new();
}