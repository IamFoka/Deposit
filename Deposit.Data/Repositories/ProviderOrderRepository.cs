using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deposit.Data.Repositories
{
    public class ProviderOrderRepository : IRepository<ProviderOrder>
    {
        private readonly DepositDbContext _context;

        public ProviderOrderRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(ProviderOrder providerOrder)
        {
            _context.ProviderOrders.Add(providerOrder);
            _context.SaveChanges();

            // if (providerOrder.ProviderOrderItems.Count == 0)
            //     return;
            
            // var providerOrderItemRepository = new ProviderOrderItemRepository(_context);

            // foreach (var providerOrderItem in providerOrder.ProviderOrderItems)
            //     providerOrderItemRepository.Add(providerOrderItem);
        }

        public IEnumerable<ProviderOrder> ListAll()
        {
            return _context.ProviderOrders.Include(o => o.Provider).Include(o => o.ProviderOrderItems).ThenInclude(i => i.Product).AsEnumerable();

        }

        public void Update(ProviderOrder entity)
        {
            _context.ProviderOrders.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var providerOrder = ListAll().FirstOrDefault(o => o.Id == guid);
            
            if (providerOrder == null)
                throw new ArgumentException("Provider order not found.");

            providerOrder.Delete();
            _context.SaveChanges();
        }
    }
}
