using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FCMPushNotification.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count();

        TEntity Get(Guid id);

        TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
