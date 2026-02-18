using System.Linq.Expressions;
using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories;

public interface IGenericRepository<TEntity> where TEntity : IDatabaseEntity
{
    Task<TEntity?> GetAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task CreateAsync(TEntity entity);
    Task CreateManyAsync(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    Task SaveAsync();
}