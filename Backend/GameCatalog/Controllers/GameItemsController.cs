using GameCatalog.Models;
using GameCatalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameItemsController : ControllerBase
{
    private readonly GameService _gameService;

    public GameItemsController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GameItem>> GetAll()
    {
        return Ok(_gameService.GetAllItems());
    }

    [HttpGet("{id}")]
    public ActionResult<GameItem> GetById(int id)
    {
        var item = _gameService.GetItemById(id);
        if (item == null)
            return NotFound();
        
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<GameItem> Create([FromBody] CreateGameItemRequest request)
    {
        _gameService.CreateItem(
            request.Name,
            request.Category,
            request.LevelRequirement,
            request.Price,
            request.Rarity
        );
        
        return Ok(new { message = "Item created successfully" });
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] CreateGameItemRequest request)
    {
        var success = _gameService.UpdateItem(
            id,
            request.Name,
            request.Category,
            request.LevelRequirement,
            request.Price,
            request.Rarity
        );

        if (!success)
            return NotFound();

        return Ok(new { message = "Item updated successfully" });
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var success = _gameService.DeleteItem(id);
        
        if (!success)
            return NotFound();

        return Ok(new { message = "Item deleted successfully" });
    }

    [HttpGet("analytics")]
    public ActionResult<GameAnalytics> GetAnalytics()
    {
        return Ok(_gameService.GetAnalytics());
    }

    [HttpPost("generate")]
    public ActionResult GenerateRandomItems([FromBody] GenerateItemsRequest request)
    {
        _gameService.GenerateRandomItems(request.Count);
        return Ok(new { message = $"Generated {request.Count} items successfully" });
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetCategories()
    {
        return Ok(Enum.GetNames<GameCategory>());
    }

    [HttpGet("rarities")]
    public ActionResult<IEnumerable<string>> GetRarities()
    {
        return Ok(Enum.GetNames<Rarity>());
    }
}

public record CreateGameItemRequest(
    string Name,
    GameCategory Category,
    int LevelRequirement,
    decimal Price,
    Rarity Rarity
);

public record GenerateItemsRequest(int Count);
