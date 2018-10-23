using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deposit.Data.Repositories
{
    public class ProviderOrderItemRepository : IRepository<ProviderOrderItem>
    {
        private readonly DepositDbContext _context;

        public ProviderOrderItemRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(ProviderOrderItem providerOrderItem)
        {
            _context.ProviderOrderItems.Add(providerOrderItem);

            foreach (var providerDeposit in providerOrderItem.Deposits)
                _context.ProviderDeposits.Add(providerDeposit);

            _context.SaveChanges();
        }

        public IEnumerable<ProviderOrderItem> ListAll()
        {
            return _context.ProviderOrderItems.Include(o => o.Product).AsEnumerable();
        }

        public void Update(ProviderOrderItem entity)
        {
            _context.ProviderOrderItems.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var providerOrderItem = ListAll().FirstOrDefault(i => i.Id == guid);
            
            if (providerOrderItem == null)
                throw new ArgumentException("Provider order item not found.");

            providerOrderItem.Delete();
            _context.SaveChanges();
        }
    }
}
