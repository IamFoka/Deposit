using System;
using System.Linq;
using System.Linq.Expressions;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deposit.Data.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly DepositDbContext _context;
        private readonly DbSet<TEntity> _set;
        public GenericRepository(DepositDbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = GetById(id);

            entity.Delete();
            Update(entity);
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return Populate(_set.AsQueryable(), includes);
        }

        public void Update(TEntity entity)
        {
            _set.Update(entity);
            _context.SaveChanges();
        }

        public TEntity GetById(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            return Populate(_set.AsQueryable(), includes)
                    .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            return Populate(_set.AsQueryable(), includes)
                    .Where(predicate);
        }
        public IQueryable<TEntity> Populate(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
            => includes.Aggregate(query, (current, include) => current.Include(include));
    }
}