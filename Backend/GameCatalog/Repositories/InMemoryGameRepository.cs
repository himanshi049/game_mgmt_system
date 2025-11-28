using GameCatalog.Models;

namespace GameCatalog.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly List<GameItem> _items;
    private int _nextId;

    public InMemoryGameRepository()
    {
        _items = new List<GameItem>();
        _nextId = 1;
        SeedGameOfThronesData();
    }

    private void SeedGameOfThronesData()
    {
        // Valyrian Steel Weapons
        Add(new GameItem { Name = "Ice (Ned Stark's Greatsword)", Category = GameCategory.ValyrianSteel, LevelRequirement = 80, Price = 50000m, Rarity = Rarity.Legendary });
        Add(new GameItem { Name = "Longclaw (Jon Snow's Bastard Sword)", Category = GameCategory.ValyrianSteel, LevelRequirement = 75, Price = 45000m, Rarity = Rarity.Legendary });
        Add(new GameItem { Name = "Oathkeeper (Brienne's Sword)", Category = GameCategory.ValyrianSteel, LevelRequirement = 70, Price = 42000m, Rarity = Rarity.Legendary });
        Add(new GameItem { Name = "Widow's Wail (Joffrey's Sword)", Category = GameCategory.ValyrianSteel, LevelRequirement = 65, Price = 40000m, Rarity = Rarity.Epic });
        
        // Dragons
        Add(new GameItem { Name = "Drogon (Black Dragon)", Category = GameCategory.Dragon, LevelRequirement = 100, Price = 100000m, Rarity = Rarity.Mythic });
        Add(new GameItem { Name = "Rhaegal (Green Dragon)", Category = GameCategory.Dragon, LevelRequirement = 100, Price = 100000m, Rarity = Rarity.Mythic });
        Add(new GameItem { Name = "Viserion (Cream Dragon)", Category = GameCategory.Dragon, LevelRequirement = 100, Price = 100000m, Rarity = Rarity.Mythic });
        
        // Crowns
        Add(new GameItem { Name = "Iron Crown of the North", Category = GameCategory.Crown, LevelRequirement = 60, Price = 25000m, Rarity = Rarity.Epic });
        Add(new GameItem { Name = "Crown of the Seven Kingdoms", Category = GameCategory.Crown, LevelRequirement = 90, Price = 75000m, Rarity = Rarity.Legendary });
        
        // Armor
        Add(new GameItem { Name = "The Hound's Helmet", Category = GameCategory.Armor, LevelRequirement = 55, Price = 15000m, Rarity = Rarity.Rare });
        Add(new GameItem { Name = "Lannister Golden Armor", Category = GameCategory.Armor, LevelRequirement = 50, Price = 18000m, Rarity = Rarity.Epic });
        Add(new GameItem { Name = "Night's Watch Black Cloak", Category = GameCategory.Armor, LevelRequirement = 30, Price = 5000m, Rarity = Rarity.Uncommon });
        
        // Artifacts
        Add(new GameItem { Name = "Dragon Eggs (Set of 3)", Category = GameCategory.Artifact, LevelRequirement = 1, Price = 1000m, Rarity = Rarity.Legendary });
        Add(new GameItem { Name = "The Iron Throne", Category = GameCategory.Artifact, LevelRequirement = 95, Price = 999999m, Rarity = Rarity.Mythic });
        Add(new GameItem { Name = "Arya's Needle", Category = GameCategory.Weapon, LevelRequirement = 20, Price = 3000m, Rarity = Rarity.Rare });
        
        // Wildling Items
        Add(new GameItem { Name = "Wildling Bone Armor", Category = GameCategory.WildlingItem, LevelRequirement = 25, Price = 2000m, Rarity = Rarity.Uncommon });
        Add(new GameItem { Name = "Giant's Bone Club", Category = GameCategory.WildlingItem, LevelRequirement = 40, Price = 8000m, Rarity = Rarity.Rare });
        
        // Potions
        Add(new GameItem { Name = "Maester's Healing Potion", Category = GameCategory.Potion, LevelRequirement = 10, Price = 500m, Rarity = Rarity.Common });
        Add(new GameItem { Name = "The Strangler Poison", Category = GameCategory.Potion, LevelRequirement = 45, Price = 12000m, Rarity = Rarity.Epic });
        Add(new GameItem { Name = "Milk of the Poppy", Category = GameCategory.Potion, LevelRequirement = 15, Price = 750m, Rarity = Rarity.Uncommon });
    }

    public void Add(GameItem item)
    {
        item.Id = _nextId++;
        item.CreatedAt = DateTime.Now;
        _items.Add(item);
    }

    public IEnumerable<GameItem> GetAll()
    {
        return _items.ToList();
    }

    public GameItem? GetById(int id)
    {
        return _items.FirstOrDefault(item => item.Id == id);
    }

    public bool Update(GameItem item)
    {
        var existingItem = GetById(item.Id);
        if (existingItem == null)
            return false;

        existingItem.Name = item.Name;
        existingItem.Category = item.Category;
        existingItem.LevelRequirement = item.LevelRequirement;
        existingItem.Price = item.Price;
        existingItem.Rarity = item.Rarity;
        // CreatedAt remains unchanged

        return true;
    }

    public bool Delete(int id)
    {
        var item = GetById(id);
        if (item == null)
            return false;

        _items.Remove(item);
        return true;
    }

    public int GetNextId()
    {
        return _nextId;
    }

    public void Clear()
    {
        _items.Clear();
        _nextId = 1;
    }
}
