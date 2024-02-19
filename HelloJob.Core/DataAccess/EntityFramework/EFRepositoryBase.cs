using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
    
namespace HelloJob.Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<T, TContext> : IRepositoryBase<T>
        where T : class
        where TContext : DbContext, new()
    {
        public async Task AddAsync(T entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChangesAsync();
        }

       

        public async Task RemoveAsync(T entity)
        {
            using var context = new TContext();
            var deleteEntity = context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChangesAsync();
        }



        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, params string[] Includes)
        {
            using var context = new TContext();

            var query = context.Set<T>().Where(expression);

            if (Includes != null)
            {
                foreach (string include in Includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync();
        }
        public IQueryable<T> GetQuery(Expression<Func<T, bool>>? expression = null)
        {
            var context = new TContext();
            return expression == null ? context.Set<T>() : context.Set<T>().Where(expression);
        }

        public async Task UpdateAsync(T entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChangesAsync();
        }

    }
}
