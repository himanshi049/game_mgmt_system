using GameCatalog.Models;
using GameCatalog.Repositories;

namespace GameCatalog.Services;

public class GameService
{
    private readonly IGameRepository _repository;
    private readonly Random _random;

    public GameService(IGameRepository repository)
    {
        _repository = repository;
        _random = new Random();
    }

    // CRUD Operations
    public void CreateItem(string name, GameCategory category, int levelRequirement, decimal price, Rarity rarity)
    {
        var item = new GameItem
        {
            Name = name,
            Category = category,
            LevelRequirement = levelRequirement,
            Price = price,
            Rarity = rarity
        };

        _repository.Add(item);
    }

    public IEnumerable<GameItem> GetAllItems()
    {
        return _repository.GetAll();
    }

    public GameItem? GetItemById(int id)
    {
        return _repository.GetById(id);
    }

    public bool UpdateItem(int id, string name, GameCategory category, int levelRequirement, decimal price, Rarity rarity)
    {
        var item = new GameItem
        {
            Id = id,
            Name = name,
            Category = category,
            LevelRequirement = levelRequirement,
            Price = price,
            Rarity = rarity
        };

        return _repository.Update(item);
    }

    public bool DeleteItem(int id)
    {
        return _repository.Delete(id);
    }

    // Data Generation
    public void GenerateRandomItems(int count)
    {
        var itemNames = new Dictionary<GameCategory, string[]>
        {
            { GameCategory.Weapon, new[] { "Arakh", "Dothraki Blade", "Westerosi Longsword", "Crossbow", "War Hammer", "Battle Axe", "Spear of the Unsullied", "Scorpion Bolt" } },
            { GameCategory.Armor, new[] { "Knight's Plate Armor", "Wildling Furs", "Kingsguard White Cloak", "Chainmail", "Leather Armor", "Dragon Scale Vest", "Unsullied Helmet", "Ironborn Armor" } },
            { GameCategory.Dragon, new[] { "Young Dragon", "Fire Drake", "Ice Dragon", "Shadow Wyrm", "Blood Dragon", "Golden Drake", "War Dragon", "Ancient Wyrm" } },
            { GameCategory.Artifact, new[] { "Dragon Glass", "Ancient Horn", "Weirwood Seed", "Wildfire Cache", "Three-Eyed Raven Token", "Faceless Coin", "Obsidian Candle", "Dragon Bone" } },
            { GameCategory.ValyrianSteel, new[] { "Valyrian Dagger", "Valyrian Longsword", "Valyrian Arakh", "Valyrian Greatsword", "Valyrian Axe", "Valyrian Spear", "Valyrian Knife", "Valyrian Chain" } },
            { GameCategory.WildlingItem, new[] { "Mammoth Tusk", "Giant's Weapon", "Bone Spear", "Wildling Bow", "Fur Cloak", "Tribal Mask", "Stone Hammer", "Bone Dagger" } },
            { GameCategory.Potion, new[] { "Maester's Remedy", "Poison Vial", "Antidote", "Sleeping Draught", "Healing Salve", "Essence of Nightshade", "Greyscale Cure", "Dragon's Breath Elixir" } },
            { GameCategory.Crown, new[] { "Dornish Coronet", "Tyrell Crown", "Baratheon Antler Crown", "Targaryen Crown", "Stark Circlet", "Lannister Diadem", "Greyjoy Kraken Crown", "Martell Sun Crown" } }
        };

        for (int i = 0; i < count; i++)
        {
            var category = (GameCategory)_random.Next(Enum.GetValues<GameCategory>().Length);
            var rarity = (Rarity)_random.Next(Enum.GetValues<Rarity>().Length);
            var nameArray = itemNames[category];
            var name = nameArray[_random.Next(nameArray.Length)];
            
            // Add uniqueness to names
            name = $"{name} #{_random.Next(1000, 9999)}";

            var levelRequirement = rarity switch
            {
                Rarity.Common => _random.Next(1, 10),
                Rarity.Uncommon => _random.Next(10, 20),
                Rarity.Rare => _random.Next(20, 40),
                Rarity.Epic => _random.Next(40, 60),
                Rarity.Legendary => _random.Next(60, 80),
                Rarity.Mythic => _random.Next(80, 100),
                _ => _random.Next(1, 50)
            };

            var basePrice = rarity switch
            {
                Rarity.Common => _random.Next(10, 100),
                Rarity.Uncommon => _random.Next(100, 500),
                Rarity.Rare => _random.Next(500, 2000),
                Rarity.Epic => _random.Next(2000, 5000),
                Rarity.Legendary => _random.Next(5000, 20000),
                Rarity.Mythic => _random.Next(20000, 100000),
                _ => _random.Next(50, 1000)
            };

            CreateItem(name, category, levelRequirement, (decimal)basePrice, rarity);
        }
    }

    // Analytics
    public GameAnalytics GetAnalytics()
    {
        var items = _repository.GetAll().ToList();

        var analytics = new GameAnalytics
        {
            TotalItems = items.Count,
            AveragePrice = items.Any() ? items.Average(i => i.Price) : 0,
            ItemsByCategory = items.GroupBy(i => i.Category)
                                   .ToDictionary(g => g.Key, g => g.Count()),
            ItemsByRarity = items.GroupBy(i => i.Rarity)
                                 .ToDictionary(g => g.Key, g => g.Count()),
            HighestLevelItem = items.OrderByDescending(i => i.LevelRequirement).FirstOrDefault()
        };

        return analytics;
    }
}
