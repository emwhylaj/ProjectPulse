using System.Linq.Expressions;

namespace ProjectPulseAPI.Core.Persistence.GenericRepo
{
    public abstract class InMemoryBaseRepository<T> : IRepository<T> where T : class
    {
        protected abstract IEnumerable<T> GetAllEntities();
        protected abstract T? GetEntityById(int id);
        protected abstract T AddEntity(T entity);
        protected abstract T? UpdateEntity(T entity);
        protected abstract bool DeleteEntityById(int id);

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await Task.FromResult(GetEntityById(id));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.FromResult(GetAllEntities().Where(e => !IsDeleted(e)));
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var compiled = predicate.Compile();
            var entities = GetAllEntities().Where(e => !IsDeleted(e) && compiled(e));
            return await Task.FromResult(entities);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var compiled = predicate.Compile();
            var entity = GetAllEntities().Where(e => !IsDeleted(e)).FirstOrDefault(compiled);
            return await Task.FromResult(entity);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await Task.FromResult(AddEntity(entity));
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            var results = new List<T>();
            foreach (var entity in entities)
            {
                results.Add(AddEntity(entity));
            }
            return await Task.FromResult(results);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            UpdateEntity(entity);
            await Task.CompletedTask;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity);
            }
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var id = (int)idProperty.GetValue(entity)!;
                DeleteEntityById(id);
            }
            await Task.CompletedTask;
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                foreach (var entity in entities)
                {
                    var id = (int)idProperty.GetValue(entity)!;
                    DeleteEntityById(id);
                }
            }
            await Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            var compiled = predicate.Compile();
            var exists = GetAllEntities().Where(e => !IsDeleted(e)).Any(compiled);
            return await Task.FromResult(exists);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            var entities = GetAllEntities().Where(e => !IsDeleted(e));
            if (predicate != null)
            {
                var compiled = predicate.Compile();
                entities = entities.Where(compiled);
            }
            return await Task.FromResult(entities.Count());
        }

        public virtual IQueryable<T> Query()
        {
            return GetAllEntities().Where(e => !IsDeleted(e)).AsQueryable();
        }

        private bool IsDeleted(T entity)
        {
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null)
            {
                var value = isDeletedProperty.GetValue(entity);
                if (value is bool boolValue)
                {
                    return boolValue;
                }
                if (value is null)
                {
                    return false;
                }
                if (value.GetType() == typeof(bool?))
                {
                    var nullableBoolValue = (bool?)value;
                    return nullableBoolValue == true;
                }
            }
            return false;
        }
    }
}