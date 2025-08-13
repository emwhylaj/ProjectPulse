using Microsoft.EntityFrameworkCore;
using ProjectPulseAPI.Infrastructure;
using ProjectPulseAPI.Core.Entities;
using System.Linq.Expressions;

namespace ProjectPulseAPI.Core.Persistence.GenericRepo
{
    public class Repository<T>(ProjectPulseDbContext context) : IRepository<T> where T : class
    {
        protected readonly ProjectPulseDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            
            if (entity == null) return null;
            
            // Check if entity is soft-deleted
            var entityType = typeof(T);
            var isDeletedProperty = entityType.GetProperty("IsDeleted");
            
            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool?))
            {
                var isDeleted = (bool?)isDeletedProperty.GetValue(entity);
                if (isDeleted == true) return null;
            }
            
            // Check if entity is active
            var isActiveProperty = entityType.GetProperty("IsActive");
            if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
            {
                var isActive = (bool)isActiveProperty.GetValue(entity)!;
                if (!isActive) return null;
            }
            
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = _dbSet.AsQueryable();
            
            // Apply soft delete filtering
            query = ApplySoftDeleteFilter(query);
            
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.Where(predicate);
            
            // Apply soft delete filtering
            query = ApplySoftDeleteFilter(query);
            
            return await query.ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.Where(predicate);
            
            // Apply soft delete filtering
            query = ApplySoftDeleteFilter(query);
            
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.Where(predicate);
            query = ApplySoftDeleteFilter(query);
            return await query.AnyAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            var query = _dbSet.AsQueryable();
            
            if (predicate != null)
                query = query.Where(predicate);
                
            query = ApplySoftDeleteFilter(query);
            
            return await query.CountAsync();
        }

        public virtual IQueryable<T> Query()
        {
            var query = _dbSet.AsQueryable();
            return ApplySoftDeleteFilter(query);
        }
        
        protected virtual IQueryable<T> ApplySoftDeleteFilter(IQueryable<T> query)
        {
            // Note: Entities with global query filters (like User, Project, UserTask) 
            // already have IsDeleted filtering applied automatically by Entity Framework.
            // Only apply additional filtering for entities without global query filters.
            
            var entityType = typeof(T);
            
            // For entities without global query filters, apply soft delete filtering
            // Check if this entity type has a global query filter by checking common entities
            if (typeof(T) == typeof(User) || 
                typeof(T).Name == "Project" || 
                typeof(T).Name == "UserTask")
            {
                // These entities have global query filters, don't add additional IsDeleted filtering
                return query;
            }
            
            // For other entities (like ProjectMember, TaskComment, etc.) that might not have global filters,
            // apply the soft delete filtering if they have the IsDeleted property
            var isDeletedProperty = entityType.GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool?))
            {
                query = query.Where(e => EF.Property<bool?>(e, "IsDeleted") != true);
            }
            
            return query;
        }
    }
}