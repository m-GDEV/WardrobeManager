using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories;

public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : class, IDatabaseEntity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected  readonly DatabaseContext _context;

        public GenericRepository(DatabaseContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        public Task<TEntity?> GetAsync(int entityId)
        {
            return _dbSet.Where(entity => entity.Id == entityId).SingleOrDefaultAsync();
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }
        
        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        
        public virtual async Task CreateManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }