using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : IDatabaseEntity
{
    Task<TEntity?> GetAsync(int entityId);
    Task<List<TEntity>> GetAllAsync(); // Used for system things (i.e. logs)
    Task CreateAsync(TEntity entity);
    Task CreateManyAsync(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    Task SaveAsync();
}