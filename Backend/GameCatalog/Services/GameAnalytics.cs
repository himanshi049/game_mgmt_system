using GameCatalog.Models;

namespace GameCatalog.Services;

public class GameAnalytics
{
    public int TotalItems { get; set; }
    public decimal AveragePrice { get; set; }
    public Dictionary<GameCategory, int> ItemsByCategory { get; set; } = new();
    public Dictionary<Rarity, int> ItemsByRarity { get; set; } = new();
    public GameItem? HighestLevelItem { get; set; }
}
