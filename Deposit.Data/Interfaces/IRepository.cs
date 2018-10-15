using System;
using System.Collections.Generic;

namespace Deposit.Data.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T t);
        T Read(Guid guid);
        List<T> ReadAll();
        void Update(Guid guid, T t);
        void Delete(Guid guid);
    }
}
