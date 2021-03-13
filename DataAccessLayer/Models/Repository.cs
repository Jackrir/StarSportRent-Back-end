using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Repository : IRepository
    {

        private readonly AppDbContext dbContext;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T Add<T>(T element)
            where T : BaseTable
        {
            T newElement = this.dbContext.Set<T>().Add(element).Entity;
            this.dbContext.SaveChanges();
            return newElement; ;
        }

        public async Task<T> AddAsync<T>(T element)
            where T : BaseTable
        {
            var newElementTask = await this.dbContext.Set<T>().AddAsync(element);
            await this.dbContext.SaveChangesAsync();
            return newElementTask.Entity;
        }

        public void AddRange<T>(IEnumerable<T> range)
             where T : BaseTable
        {
            this.dbContext.Set<T>().AddRange(range);
            this.dbContext.SaveChanges();
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> range)
            where T : BaseTable
        {
            await this.dbContext.Set<T>().AddRangeAsync(range);
            await this.dbContext.SaveChangesAsync();
        }

        public void Delete<T>(T element)
            where T : BaseTable
        {
            this.dbContext.Set<T>().Remove(element);
            this.dbContext.SaveChanges();
        }

        public Task DeleteAsync<T>(T element)
            where T : BaseTable
        {
            this.dbContext.Set<T>().Remove(element);
            return this.dbContext.SaveChangesAsync();
        }

        public void DeleteRange<T>(IEnumerable<T> range)
            where T : BaseTable
        {
            this.dbContext.Set<T>().RemoveRange(range);
            this.dbContext.SaveChanges();
        }

        public Task DeleteRangeAsync<T>(IEnumerable<T> range)
             where T : BaseTable
        {
            this.dbContext.Set<T>().RemoveRange(range);
            return this.dbContext.SaveChangesAsync();
        }

        private IQueryable<T> Include<T>(bool tracking, params Expression<Func<T, object>>[] includeProperties)
            where T : BaseTable
        {
            if (tracking)
            {
                IQueryable<T> query = this.dbContext.Set<T>();
                return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            else
            {
                IQueryable<T> query = this.dbContext.Set<T>().AsNoTracking();
                return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
        }

        public T Get<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
            where T : BaseTable
        {
            var query = this.Include(tracking, includeProperties);
            return query.AsEnumerable().Where(e => predicate(e)).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
            where T : BaseTable
        {
            var query = await this.Include(tracking, includeProperties).ToListAsync();
            return query.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetRange<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
            where T : BaseTable
        {
            var query = this.Include(tracking, includeProperties);
            return query.Where(predicate);
        }

        public async Task<IEnumerable<T>> GetRangeAsync<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
            where T : BaseTable
        {
            var query = await this.Include(tracking, includeProperties).ToListAsync();
            return query.Where(predicate);
        }

        public void Update<T>(T element)
            where T : BaseTable
        {
            this.dbContext.Set<T>().Update(element);
            this.dbContext.SaveChanges();
        }

        public Task UpdateAsync<T>(T element)
            where T : BaseTable
        {
            this.dbContext.Set<T>().Update(element);
            return this.dbContext.SaveChangesAsync();
        }

        public void UpdateRange<T>(IEnumerable<T> range)
            where T : BaseTable
        {
            this.dbContext.Set<T>().UpdateRange(range);
            this.dbContext.SaveChanges();
        }

        public Task UpdateRangeAsync<T>(IEnumerable<T> range)
            where T : BaseTable
        {
            this.dbContext.Set<T>().UpdateRange(range);
            return this.dbContext.SaveChangesAsync();
        }
    }
}
