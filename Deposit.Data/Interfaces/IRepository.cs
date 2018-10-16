using System;
using System.Collections.Generic;

namespace Deposit.Data.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        IEnumerable<T> ListAll();
        void Update(T entity);
        void Delete(Guid guid);
    }
}
