using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Domain.DataAccess.Abstract;
using OnlineAuctionApp.Domain.Entities.Abstract;
using OnlineAuctionApp.Infrastructure.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Infrastructure.DataAccess.Concrete
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrderContext _orderContext;
        public Repository(OrderContext orderContext) => (_orderContext) = (orderContext);

        public async Task<T> AddAsync(T entity)
        {
            await _orderContext.Set<T>().AddAsync(entity).ConfigureAwait(false);

            await _orderContext.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _orderContext.Set<T>().Remove(entity);

            await _orderContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _orderContext.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate) => await _orderContext.Set<T>().Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _orderContext.Set<T>();

            if (disableTracking is true) query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            if (predicate is not null) query = query.Where(predicate);

            if (orderBy is not null) return await orderBy(query).ToListAsync().ConfigureAwait(false);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id) => await _orderContext.Set<T>().FindAsync(id).ConfigureAwait(false);

        public async Task UpdateAsync(T entity)
        {
            _orderContext.Entry(entity).State = EntityState.Modified;

            await _orderContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
