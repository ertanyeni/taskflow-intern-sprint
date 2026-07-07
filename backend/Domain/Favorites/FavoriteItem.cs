namespace TaskFlow.Domain.Favorites;

/// <summary>
/// Favori: BİZE ait veri (kullanıcının kaydettiği kayıt) → bu bir Domain entity'sidir
/// ve Postgres'te yaşar (Meals/Pokémon aksine). Module alanı sayesinde farklı modüller
/// aynı tabloyu kullanabilir ama (Module, ExternalId) çifti benzersizdir.
/// </summary>
public class FavoriteItem
{
    public Guid Id { get; set; }

    /// <summary>Hangi modül: "meals", "pokedex", "library" ...</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>Dış kaynaktaki kimlik (örn. TheMealDB idMeal).</summary>
    public string ExternalId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
