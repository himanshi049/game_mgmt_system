using GameCatalog.Models;

namespace GameCatalog.Tests;

public class GameItemTests
{
    [Fact]
    public void GameItem_DefaultInitialization_HasCorrectDefaults()
    {
        // Arrange & Act
        var item = new GameItem();

        // Assert
        Assert.Equal(0, item.Id);
        Assert.Equal(string.Empty, item.Name);
        Assert.Equal(0, item.LevelRequirement);
        Assert.Equal(0, item.Price);
        Assert.Equal(default(DateTime), item.CreatedAt);
    }

    [Fact]
    public void GameItem_SetProperties_StoresCorrectly()
    {
        // Arrange
        var item = new GameItem
        {
            Id = 1,
            Name = "Excalibur",
            Category = GameCategory.Weapon,
            LevelRequirement = 50,
            Price = 5000m,
            Rarity = Rarity.Legendary,
            CreatedAt = DateTime.Now
        };

        // Act & Assert
        Assert.Equal(1, item.Id);
        Assert.Equal("Excalibur", item.Name);
        Assert.Equal(GameCategory.Weapon, item.Category);
        Assert.Equal(50, item.LevelRequirement);
        Assert.Equal(5000m, item.Price);
        Assert.Equal(Rarity.Legendary, item.Rarity);
    }

    [Fact]
    public void ToString_WithValidItem_ReturnsFormattedString()
    {
        // Arrange
        var now = DateTime.Now;
        var item = new GameItem
        {
            Id = 5,
            Name = "Dragon Egg",
            Category = GameCategory.Artifact,
            LevelRequirement = 1,
            Price = 999m,
            Rarity = Rarity.Epic,
            CreatedAt = now
        };

        // Act
        var result = item.ToString();

        // Assert
        Assert.Contains("[5]", result);
        Assert.Contains("Dragon Egg", result);
        Assert.Contains("Artifact", result);
        Assert.Contains("Level 1", result);
        Assert.Contains("999", result);
        Assert.Contains("Epic", result);
    }

    [Theory]
    [InlineData(GameCategory.Weapon)]
    [InlineData(GameCategory.Armor)]
    [InlineData(GameCategory.Dragon)]
    [InlineData(GameCategory.Crown)]
    public void GameItem_AllCategories_CanBeAssigned(GameCategory category)
    {
        // Arrange & Act
        var item = new GameItem { Category = category };

        // Assert
        Assert.Equal(category, item.Category);
    }

    [Theory]
    [InlineData(Rarity.Common)]
    [InlineData(Rarity.Uncommon)]
    [InlineData(Rarity.Rare)]
    [InlineData(Rarity.Epic)]
    [InlineData(Rarity.Legendary)]
    [InlineData(Rarity.Mythic)]
    public void GameItem_AllRarities_CanBeAssigned(Rarity rarity)
    {
        // Arrange & Act
        var item = new GameItem { Rarity = rarity };

        // Assert
        Assert.Equal(rarity, item.Rarity);
    }

    [Fact]
    public void GameItem_PriceCanBeDecimal_HandlesDecimalCorrectly()
    {
        // Arrange & Act
        var item = new GameItem { Price = 1234.56m };

        // Assert
        Assert.Equal(1234.56m, item.Price);
    }
}
