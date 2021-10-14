using FCMPushNotification.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FCMPushNotification.Repository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly SampleDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(SampleDbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
            _context.SaveChanges();
        }

        public int Count()
        {
            return _entities.Count();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public TEntity Get(Guid id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
            _context.SaveChanges();
        }
    }
}
