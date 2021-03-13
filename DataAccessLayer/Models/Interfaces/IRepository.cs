using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository
    {
        IEnumerable<T> GetRange<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseTable;

        T Get<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseTable;

        T Add<T>(T element) where T : BaseTable;

        void AddRange<T>(IEnumerable<T> range) where T : BaseTable;

        void DeleteRange<T>(IEnumerable<T> range) where T : BaseTable;

        void Delete<T>(T element) where T : BaseTable;

        void Update<T>(T element) where T : BaseTable;

        void UpdateRange<T>(IEnumerable<T> range) where T : BaseTable;

        Task<IEnumerable<T>> GetRangeAsync<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseTable;

        Task<T> GetAsync<T>(bool tracking, Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties) where T : BaseTable;

        Task<T> AddAsync<T>(T element) where T : BaseTable;

        Task AddRangeAsync<T>(IEnumerable<T> range) where T : BaseTable;

        Task DeleteRangeAsync<T>(IEnumerable<T> range) where T : BaseTable;

        Task DeleteAsync<T>(T element) where T : BaseTable;

        Task UpdateAsync<T>(T element) where T : BaseTable;

        Task UpdateRangeAsync<T>(IEnumerable<T> range) where T : BaseTable;
    }
}
