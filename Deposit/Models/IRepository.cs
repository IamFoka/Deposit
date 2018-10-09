using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
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
