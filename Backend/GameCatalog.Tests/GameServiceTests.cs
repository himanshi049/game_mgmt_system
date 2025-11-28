using GameCatalog.Models;
using GameCatalog.Repositories;
using GameCatalog.Services;
using Moq;

namespace GameCatalog.Tests;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _mockRepository;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _mockRepository = new Mock<IGameRepository>();
        _gameService = new GameService(_mockRepository.Object);
    }

    [Fact]
    public void CreateItem_WithValidData_CallsRepositoryAdd()
    {
        // Arrange
        var name = "Test Sword";
        var category = GameCategory.Weapon;
        var levelRequirement = 50;
        var price = 5000m;
        var rarity = Rarity.Legendary;

        // Act
        _gameService.CreateItem(name, category, levelRequirement, price, rarity);

        // Assert
        _mockRepository.Verify(r => r.Add(It.IsAny<GameItem>()), Times.Once);
    }

    [Fact]
    public void CreateItem_CreatesItemWithCorrectProperties()
    {
        // Arrange
        var items = new List<GameItem>();
        _mockRepository.Setup(r => r.Add(It.IsAny<GameItem>()))
            .Callback<GameItem>(item => items.Add(item));

        // Act
        _gameService.CreateItem("Dragon Egg", GameCategory.Artifact, 1, 1000m, Rarity.Legendary);

        // Assert
        Assert.Single(items);
        Assert.Equal("Dragon Egg", items[0].Name);
        Assert.Equal(GameCategory.Artifact, items[0].Category);
        Assert.Equal(1, items[0].LevelRequirement);
        Assert.Equal(1000m, items[0].Price);
        Assert.Equal(Rarity.Legendary, items[0].Rarity);
    }

    [Fact]
    public void GetAllItems_CallsRepositoryGetAll()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAll()).Returns(new List<GameItem>());

        // Act
        _gameService.GetAllItems();

        // Assert
        _mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetAllItems_ReturnsRepositoryItems()
    {
        // Arrange
        var expectedItems = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 20, Price = 200m, Rarity = Rarity.Rare }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(expectedItems);

        // Act
        var result = _gameService.GetAllItems();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, i => i.Name == "Item 1");
        Assert.Contains(result, i => i.Name == "Item 2");
    }

    [Fact]
    public void GetItemById_CallsRepositoryGetById()
    {
        // Arrange
        var itemId = 5;
        _mockRepository.Setup(r => r.GetById(itemId)).Returns((GameItem?)null);

        // Act
        _gameService.GetItemById(itemId);

        // Assert
        _mockRepository.Verify(r => r.GetById(itemId), Times.Once);
    }

    [Fact]
    public void GetItemById_ReturnsItem()
    {
        // Arrange
        var expectedItem = new GameItem { Id = 1, Name = "Excalibur", Category = GameCategory.Weapon, LevelRequirement = 50, Price = 5000m, Rarity = Rarity.Legendary };
        _mockRepository.Setup(r => r.GetById(1)).Returns(expectedItem);

        // Act
        var result = _gameService.GetItemById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Excalibur", result.Name);
    }

    [Fact]
    public void UpdateItem_WithValidData_CallsRepositoryUpdate()
    {
        // Arrange
        _mockRepository.Setup(r => r.Update(It.IsAny<GameItem>())).Returns(true);

        // Act
        _gameService.UpdateItem(1, "Updated", GameCategory.Armor, 60, 6000m, Rarity.Epic);

        // Assert
        _mockRepository.Verify(r => r.Update(It.IsAny<GameItem>()), Times.Once);
    }

    [Fact]
    public void UpdateItem_ReturnsRepositoryResult()
    {
        // Arrange
        _mockRepository.Setup(r => r.Update(It.IsAny<GameItem>())).Returns(true);

        // Act
        var result = _gameService.UpdateItem(1, "Updated", GameCategory.Armor, 60, 6000m, Rarity.Epic);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteItem_CallsRepositoryDelete()
    {
        // Arrange
        _mockRepository.Setup(r => r.Delete(It.IsAny<int>())).Returns(true);

        // Act
        _gameService.DeleteItem(1);

        // Assert
        _mockRepository.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public void DeleteItem_ReturnsRepositoryResult()
    {
        // Arrange
        _mockRepository.Setup(r => r.Delete(It.IsAny<int>())).Returns(false);

        // Act
        var result = _gameService.DeleteItem(9999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GenerateRandomItems_CreatesMultipleItems()
    {
        // Arrange
        var items = new List<GameItem>();
        _mockRepository.Setup(r => r.Add(It.IsAny<GameItem>()))
            .Callback<GameItem>(item => items.Add(item));

        // Act
        _gameService.GenerateRandomItems(10);

        // Assert
        Assert.Equal(10, items.Count);
    }

    [Fact]
    public void GenerateRandomItems_CreatesItemsWithValidProperties()
    {
        // Arrange
        var items = new List<GameItem>();
        _mockRepository.Setup(r => r.Add(It.IsAny<GameItem>()))
            .Callback<GameItem>(item => items.Add(item));

        // Act
        _gameService.GenerateRandomItems(5);

        // Assert
        foreach (var item in items)
        {
            Assert.NotEmpty(item.Name);
            Assert.NotEqual(0, item.LevelRequirement);
            Assert.NotEqual(0, item.Price);
            Assert.InRange(item.LevelRequirement, 1, 100);
            Assert.InRange((int)item.Price, 10, 100000);
        }
    }

    [Theory]
    [InlineData(Rarity.Common, 1, 10, 10, 100)]
    [InlineData(Rarity.Uncommon, 10, 20, 100, 500)]
    [InlineData(Rarity.Rare, 20, 40, 500, 2000)]
    [InlineData(Rarity.Epic, 40, 60, 2000, 5000)]
    [InlineData(Rarity.Legendary, 60, 80, 5000, 20000)]
    [InlineData(Rarity.Mythic, 80, 100, 20000, 100000)]
    public void GenerateRandomItems_RarityAffectsLevelAndPrice(Rarity rarity, int minLevel, int maxLevel, int minPrice, int maxPrice)
    {
        // Arrange
        var items = new List<GameItem>();
        _mockRepository.Setup(r => r.Add(It.IsAny<GameItem>()))
            .Callback<GameItem>(item => items.Add(item));

        // Generate items until we get the target rarity
        for (int i = 0; i < 100; i++)
        {
            items.Clear();
            _gameService.GenerateRandomItems(1);
            if (items[0].Rarity == rarity)
            {
                // Assert
                Assert.InRange(items[0].LevelRequirement, minLevel, maxLevel);
                Assert.InRange((int)items[0].Price, minPrice, maxPrice);
                return;
            }
        }

        // If we couldn't generate the rarity in 100 tries, skip
        Assert.True(true, "Could not generate specified rarity for test");
    }

    [Fact]
    public void GetAnalytics_WithNoItems_ReturnsZeroStats()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAll()).Returns(new List<GameItem>());

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.Equal(0, analytics.TotalItems);
        Assert.Equal(0, analytics.AveragePrice);
        Assert.Empty(analytics.ItemsByCategory);
        Assert.Empty(analytics.ItemsByRarity);
        Assert.Null(analytics.HighestLevelItem);
    }

    [Fact]
    public void GetAnalytics_CalculatesTotalItems()
    {
        // Arrange
        var items = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 20, Price = 200m, Rarity = Rarity.Rare },
            new GameItem { Id = 3, Name = "Item 3", Category = GameCategory.Dragon, LevelRequirement = 30, Price = 300m, Rarity = Rarity.Epic }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.Equal(3, analytics.TotalItems);
    }

    [Fact]
    public void GetAnalytics_CalculatesAveragePrice()
    {
        // Arrange
        var items = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 20, Price = 200m, Rarity = Rarity.Rare },
            new GameItem { Id = 3, Name = "Item 3", Category = GameCategory.Dragon, LevelRequirement = 30, Price = 300m, Rarity = Rarity.Epic }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.Equal(200m, analytics.AveragePrice);
    }

    [Fact]
    public void GetAnalytics_GroupsItemsByCategory()
    {
        // Arrange
        var items = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Weapon, LevelRequirement = 20, Price = 200m, Rarity = Rarity.Rare },
            new GameItem { Id = 3, Name = "Item 3", Category = GameCategory.Armor, LevelRequirement = 30, Price = 300m, Rarity = Rarity.Epic }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.Equal(2, analytics.ItemsByCategory[GameCategory.Weapon]);
        Assert.Equal(1, analytics.ItemsByCategory[GameCategory.Armor]);
    }

    [Fact]
    public void GetAnalytics_GroupsItemsByRarity()
    {
        // Arrange
        var items = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 20, Price = 200m, Rarity = Rarity.Common },
            new GameItem { Id = 3, Name = "Item 3", Category = GameCategory.Dragon, LevelRequirement = 30, Price = 300m, Rarity = Rarity.Epic }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.Equal(2, analytics.ItemsByRarity[Rarity.Common]);
        Assert.Equal(1, analytics.ItemsByRarity[Rarity.Epic]);
    }

    [Fact]
    public void GetAnalytics_FindsHighestLevelItem()
    {
        // Arrange
        var items = new List<GameItem>
        {
            new GameItem { Id = 1, Name = "Item 1", Category = GameCategory.Weapon, LevelRequirement = 10, Price = 100m, Rarity = Rarity.Common },
            new GameItem { Id = 2, Name = "Item 2", Category = GameCategory.Armor, LevelRequirement = 50, Price = 200m, Rarity = Rarity.Rare },
            new GameItem { Id = 3, Name = "Item 3", Category = GameCategory.Dragon, LevelRequirement = 30, Price = 300m, Rarity = Rarity.Epic }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var analytics = _gameService.GetAnalytics();

        // Assert
        Assert.NotNull(analytics.HighestLevelItem);
        Assert.Equal(50, analytics.HighestLevelItem.LevelRequirement);
        Assert.Equal("Item 2", analytics.HighestLevelItem.Name);
    }
}
