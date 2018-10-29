using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Deposit.Domain.Entities;

namespace Deposit.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
        TEntity GetById(Guid id, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        void Update(TEntity entity);
        void Delete(Guid guid);
        IQueryable<TEntity> Populate(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes);
    }
}
