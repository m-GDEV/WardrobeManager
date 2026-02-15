using System.Linq.Expressions;
using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories;

public interface IGenericRepository<TEntity> where TEntity : DatabaseEntity
{
    Task<TEntity?> GetAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task CreateAsync(TEntity entity);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    Task SaveAsync();
}