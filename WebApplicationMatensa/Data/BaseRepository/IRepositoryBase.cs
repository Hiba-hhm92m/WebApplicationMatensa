using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebApplicationMatensa.Data.BaseRepository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate = null, string Include = null, bool hasTracking = true, bool hasTid = true);
        T GetById(int id, bool isTracking = true);
        void Insert(T entity, bool hasTid = true);
        void Delete(T entity);
        void Update();
        void UpdateEntity(T entity);
    }
}
