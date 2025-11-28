using GameCatalog.Models;

namespace GameCatalog.Repositories;

public interface IGameRepository
{
    // Create
    void Add(GameItem item);
    
    // Read
    IEnumerable<GameItem> GetAll();
    GameItem? GetById(int id);
    
    // Update
    bool Update(GameItem item);
    
    // Delete
    bool Delete(int id);
    
    // Utility
    int GetNextId();
    void Clear();
}
