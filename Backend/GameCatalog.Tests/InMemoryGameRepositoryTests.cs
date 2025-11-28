using GameCatalog.Models;
using GameCatalog.Repositories;

namespace GameCatalog.Tests;

public class InMemoryGameRepositoryTests
{
    private InMemoryGameRepository _repository;

    public InMemoryGameRepositoryTests()
    {
        _repository = new InMemoryGameRepository();
    }

    [Fact]
    public void Repository_Initialize_WithSeededData()
    {
        // Act
        var items = _repository.GetAll();

        // Assert
        Assert.NotEmpty(items);
        Assert.True(items.Count() > 0, "Repository should have seeded data");
    }

    [Fact]
    public void Add_NewItem_IncrementsIdAndAssignsCreatedAt()
    {
        // Arrange
        _repository.Clear();
        var item = new GameItem
        {
            Name = "Test Sword",
            Category = GameCategory.Weapon,
            LevelRequirement = 10,
            Price = 500m,
            Rarity = Rarity.Common
        };

        // Act
        _repository.Add(item);

        // Assert
        Assert.Equal(1, item.Id);
        Assert.NotEqual(default(DateTime), item.CreatedAt);
    }

    [Fact]
    public void Add_MultipleItems_AssignsUniqueIds()
    {
        // Arrange
        _repository.Clear();
        var item1 = new GameItem { Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 1, Price = 100m, Rarity = Rarity.Common };
        var item2 = new GameItem { Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 2, Price = 200m, Rarity = Rarity.Uncommon };

        // Act
        _repository.Add(item1);
        _repository.Add(item2);

        // Assert
        Assert.Equal(1, item1.Id);
        Assert.Equal(2, item2.Id);
    }

    [Fact]
    public void GetAll_ReturnsAllItems()
    {
        // Arrange
        _repository.Clear();
        _repository.Add(new GameItem { Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 1, Price = 100m, Rarity = Rarity.Common });
        _repository.Add(new GameItem { Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 2, Price = 200m, Rarity = Rarity.Uncommon });

        // Act
        var items = _repository.GetAll();

        // Assert
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public void GetById_WithValidId_ReturnsItem()
    {
        // Arrange
        _repository.Clear();
        var item = new GameItem { Name = "Test Item", Category = GameCategory.Weapon, LevelRequirement = 5, Price = 500m, Rarity = Rarity.Rare };
        _repository.Add(item);

        // Act
        var retrieved = _repository.GetById(1);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("Test Item", retrieved.Name);
        Assert.Equal(1, retrieved.Id);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Act
        var retrieved = _repository.GetById(9999);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public void Update_ExistingItem_ModifiesProperties()
    {
        // Arrange
        _repository.Clear();
        var item = new GameItem { Name = "Original", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 1000m, Rarity = Rarity.Epic };
        _repository.Add(item);

        var updatedItem = new GameItem
        {
            Id = 1,
            Name = "Updated",
            Category = GameCategory.Armor,
            LevelRequirement = 20,
            Price = 2000m,
            Rarity = Rarity.Legendary
        };

        // Act
        var success = _repository.Update(updatedItem);
        var result = _repository.GetById(1);

        // Assert
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
        Assert.Equal(GameCategory.Armor, result.Category);
        Assert.Equal(20, result.LevelRequirement);
        Assert.Equal(2000m, result.Price);
        Assert.Equal(Rarity.Legendary, result.Rarity);
    }

    [Fact]
    public void Update_NonExistentItem_ReturnsFalse()
    {
        // Arrange
        var nonExistentItem = new GameItem
        {
            Id = 9999,
            Name = "Ghost Item",
            Category = GameCategory.Crown,
            LevelRequirement = 50,
            Price = 50000m,
            Rarity = Rarity.Mythic
        };

        // Act
        var success = _repository.Update(nonExistentItem);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void Delete_ExistingItem_RemovesIt()
    {
        // Arrange
        _repository.Clear();
        var item = new GameItem { Name = "To Delete", Category = GameCategory.Weapon, LevelRequirement = 5, Price = 500m, Rarity = Rarity.Common };
        _repository.Add(item);

        // Act
        var success = _repository.Delete(1);
        var retrieved = _repository.GetById(1);

        // Assert
        Assert.True(success);
        Assert.Null(retrieved);
    }

    [Fact]
    public void Delete_NonExistentItem_ReturnsFalse()
    {
        // Act
        var success = _repository.Delete(9999);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void Delete_MultipleItems_OnlyRemovesTargetItem()
    {
        // Arrange
        _repository.Clear();
        var item1 = new GameItem { Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 1, Price = 100m, Rarity = Rarity.Common };
        var item2 = new GameItem { Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 2, Price = 200m, Rarity = Rarity.Uncommon };
        _repository.Add(item1);
        _repository.Add(item2);

        // Act
        _repository.Delete(1);

        // Assert
        Assert.Null(_repository.GetById(1));
        Assert.NotNull(_repository.GetById(2));
        Assert.Single(_repository.GetAll());
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        // Arrange
        _repository.Clear();
        _repository.Add(new GameItem { Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 1, Price = 100m, Rarity = Rarity.Common });
        _repository.Add(new GameItem { Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 2, Price = 200m, Rarity = Rarity.Uncommon });

        // Act
        _repository.Clear();

        // Assert
        Assert.Empty(_repository.GetAll());
    }

    [Fact]
    public void GetNextId_ReturnsIncrementingId()
    {
        // Arrange
        _repository.Clear();

        // Act
        var id1 = _repository.GetNextId();
        _repository.Add(new GameItem { Name = "Item", Category = GameCategory.Weapon, LevelRequirement = 1, Price = 100m, Rarity = Rarity.Common });
        var id2 = _repository.GetNextId();

        // Assert
        Assert.Equal(1, id1);
        Assert.Equal(2, id2);
    }
}
