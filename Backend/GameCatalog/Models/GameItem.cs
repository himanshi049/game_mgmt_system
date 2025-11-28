namespace GameCatalog.Models;

public class GameItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GameCategory Category { get; set; }
    public int LevelRequirement { get; set; }
    public decimal Price { get; set; }
    public Rarity Rarity { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Name} - {Category} | Level {LevelRequirement} | {Price:C} | {Rarity} | Created: {CreatedAt:yyyy-MM-dd}";
    }
}
