using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        private readonly DepositDbContext _context;

        public ProviderRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(Provider provider)
        {
            _context.Providers.Add(provider);
            _context.SaveChanges();
        }

        public IEnumerable<Provider> ListAll()
        {
            return _context.Providers.AsEnumerable();
        }

        public void Update(Provider entity)
        {
            _context.Providers.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var provider = ListAll().FirstOrDefault(p => p.Id == guid);
            
            if (provider == null)
                throw new ArgumentException("Provider not found.");
            
            provider.Delete();
            Update(provider);
        }
    }
}
