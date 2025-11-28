using GameCatalog.Models;
using GameCatalog.Services;

namespace GameCatalog.UI;

public class ConsoleUI
{
    private readonly GameService _gameService;

    public ConsoleUI(GameService gameService)
    {
        _gameService = gameService;
    }

    public void Run()
    {
        while (true)
        {
            ShowMainMenu();
            var choice = GetInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    ManageItemsMenu();
                    break;
                case "2":
                    ViewAnalytics();
                    break;
                case "3":
                    GenerateDataMenu();
                    break;
                case "4":
                    Console.WriteLine("\nValar Morghulis - All men must die. Farewell!");
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid choice. Please try again.");
                    WaitForKey();
                    break;
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║   GAME OF THRONES ITEM CATALOG       ║");
        Console.WriteLine("║        Winter is Coming...           ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("1. Manage Items (CRUD)");
        Console.WriteLine("2. View Analytics");
        Console.WriteLine("3. Generate Sample GoT Data");
        Console.WriteLine("4. Exit");
        Console.WriteLine();
    }

    private void ManageItemsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║         MANAGE ITEMS MENU            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. Create New Item");
            Console.WriteLine("2. View All Items");
            Console.WriteLine("3. View Item by ID");
            Console.WriteLine("4. Update Item");
            Console.WriteLine("5. Delete Item");
            Console.WriteLine("6. Back to Main Menu");
            Console.WriteLine();

            var choice = GetInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    CreateItem();
                    break;
                case "2":
                    ViewAllItems();
                    break;
                case "3":
                    ViewItemById();
                    break;
                case "4":
                    UpdateItem();
                    break;
                case "5":
                    DeleteItem();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("\n❌ Invalid choice. Please try again.");
                    WaitForKey();
                    break;
            }
        }
    }

    private void CreateItem()
    {
        Console.Clear();
        Console.WriteLine("═══ CREATE NEW ITEM ═══\n");

        var name = GetInput("Enter item name: ");
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Name cannot be empty.");
            WaitForKey();
            return;
        }

        var category = GetEnumInput<GameCategory>("Select category");
        if (category == null)
        {
            WaitForKey();
            return;
        }

        var level = GetIntInput("Enter level requirement (1-100): ", 1, 100);
        if (level == null)
        {
            WaitForKey();
            return;
        }

        var price = GetDecimalInput("Enter price: ", 0, 1000000);
        if (price == null)
        {
            WaitForKey();
            return;
        }

        var rarity = GetEnumInput<Rarity>("Select rarity");
        if (rarity == null)
        {
            WaitForKey();
            return;
        }

        _gameService.CreateItem(name, category.Value, level.Value, price.Value, rarity.Value);
        Console.WriteLine("\n✓ Item created successfully!");
        WaitForKey();
    }

    private void ViewAllItems()
    {
        Console.Clear();
        Console.WriteLine("═══ ALL ITEMS ═══\n");

        var items = _gameService.GetAllItems().ToList();
        
        if (!items.Any())
        {
            Console.WriteLine("No items found in the catalog.");
        }
        else
        {
            Console.WriteLine($"Total Items: {items.Count}\n");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        WaitForKey();
    }

    private void ViewItemById()
    {
        Console.Clear();
        Console.WriteLine("═══ VIEW ITEM BY ID ═══\n");

        var id = GetIntInput("Enter item ID: ", 1, int.MaxValue);
        if (id == null)
        {
            WaitForKey();
            return;
        }

        var item = _gameService.GetItemById(id.Value);
        if (item == null)
        {
            Console.WriteLine($"\n❌ Item with ID {id} not found.");
        }
        else
        {
            Console.WriteLine("\n" + item);
        }

        WaitForKey();
    }

    private void UpdateItem()
    {
        Console.Clear();
        Console.WriteLine("═══ UPDATE ITEM ═══\n");

        var id = GetIntInput("Enter item ID to update: ", 1, int.MaxValue);
        if (id == null)
        {
            WaitForKey();
            return;
        }

        var existingItem = _gameService.GetItemById(id.Value);
        if (existingItem == null)
        {
            Console.WriteLine($"\n❌ Item with ID {id} not found.");
            WaitForKey();
            return;
        }

        Console.WriteLine($"\nCurrent item: {existingItem}\n");

        var name = GetInput($"Enter new name (current: {existingItem.Name}): ");
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Name cannot be empty.");
            WaitForKey();
            return;
        }

        var category = GetEnumInput<GameCategory>($"Select new category (current: {existingItem.Category})");
        if (category == null)
        {
            WaitForKey();
            return;
        }

        var level = GetIntInput($"Enter new level requirement (current: {existingItem.LevelRequirement}): ", 1, 100);
        if (level == null)
        {
            WaitForKey();
            return;
        }

        var price = GetDecimalInput($"Enter new price (current: {existingItem.Price:C}): ", 0, 1000000);
        if (price == null)
        {
            WaitForKey();
            return;
        }

        var rarity = GetEnumInput<Rarity>($"Select new rarity (current: {existingItem.Rarity})");
        if (rarity == null)
        {
            WaitForKey();
            return;
        }

        var success = _gameService.UpdateItem(id.Value, name, category.Value, level.Value, price.Value, rarity.Value);
        if (success)
        {
            Console.WriteLine("\n✓ Item updated successfully!");
        }
        else
        {
            Console.WriteLine("\n❌ Failed to update item.");
        }

        WaitForKey();
    }

    private void DeleteItem()
    {
        Console.Clear();
        Console.WriteLine("═══ DELETE ITEM ═══\n");

        var id = GetIntInput("Enter item ID to delete: ", 1, int.MaxValue);
        if (id == null)
        {
            WaitForKey();
            return;
        }

        var item = _gameService.GetItemById(id.Value);
        if (item == null)
        {
            Console.WriteLine($"\n❌ Item with ID {id} not found.");
            WaitForKey();
            return;
        }

        Console.WriteLine($"\nItem to delete: {item}");
        var confirm = GetInput("\nAre you sure you want to delete this item? (yes/no): ");

        if (confirm.ToLower() == "yes" || confirm.ToLower() == "y")
        {
            var success = _gameService.DeleteItem(id.Value);
            if (success)
            {
                Console.WriteLine("\n✓ Item deleted successfully!");
            }
            else
            {
                Console.WriteLine("\n❌ Failed to delete item.");
            }
        }
        else
        {
            Console.WriteLine("\nDeletion cancelled.");
        }

        WaitForKey();
    }

    private void ViewAnalytics()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║          ANALYTICS DASHBOARD         ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.WriteLine();

        var analytics = _gameService.GetAnalytics();

        Console.WriteLine($"📊 Total Items: {analytics.TotalItems}");
        Console.WriteLine($"💰 Average Price: {analytics.AveragePrice:C}");
        Console.WriteLine();

        if (analytics.ItemsByCategory.Any())
        {
            Console.WriteLine("📦 Items by Category:");
            foreach (var kvp in analytics.ItemsByCategory.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"   {kvp.Key,-15} : {kvp.Value,3} items");
            }
            Console.WriteLine();
        }

        if (analytics.ItemsByRarity.Any())
        {
            Console.WriteLine("✨ Items by Rarity:");
            foreach (var kvp in analytics.ItemsByRarity.OrderBy(x => (int)x.Key))
            {
                Console.WriteLine($"   {kvp.Key,-15} : {kvp.Value,3} items");
            }
            Console.WriteLine();
        }

        if (analytics.HighestLevelItem != null)
        {
            Console.WriteLine("🏆 Highest Level Requirement Item:");
            Console.WriteLine($"   {analytics.HighestLevelItem}");
        }

        WaitForKey();
    }

    private void GenerateDataMenu()
    {
        Console.Clear();
        Console.WriteLine("═══ GENERATE SAMPLE DATA ═══\n");

        var count = GetIntInput("How many items to generate? (1-1000): ", 1, 1000);
        if (count == null)
        {
            WaitForKey();
            return;
        }

        Console.WriteLine($"\nGenerating {count} items...");
        _gameService.GenerateRandomItems(count.Value);
        Console.WriteLine($"✓ Successfully generated {count} items!");

        WaitForKey();
    }

    // Helper methods for input validation
    private string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private int? GetIntInput(string prompt, int min, int max)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (int.TryParse(input, out int result))
        {
            if (result >= min && result <= max)
            {
                return result;
            }
            else
            {
                Console.WriteLine($"❌ Value must be between {min} and {max}.");
                return null;
            }
        }

        Console.WriteLine("❌ Invalid number format.");
        return null;
    }

    private decimal? GetDecimalInput(string prompt, decimal min, decimal max)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (decimal.TryParse(input, out decimal result))
        {
            if (result >= min && result <= max)
            {
                return result;
            }
            else
            {
                Console.WriteLine($"❌ Value must be between {min} and {max}.");
                return null;
            }
        }

        Console.WriteLine("❌ Invalid number format.");
        return null;
    }

    private T? GetEnumInput<T>(string prompt) where T : struct, Enum
    {
        Console.WriteLine($"\n{prompt}:");
        var values = Enum.GetValues<T>();
        
        for (int i = 0; i < values.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {values[i]}");
        }

        Console.Write("\nEnter choice: ");
        var input = Console.ReadLine()?.Trim();

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= values.Length)
        {
            return values[choice - 1];
        }

        Console.WriteLine("❌ Invalid choice.");
        return null;
    }

    private void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}
