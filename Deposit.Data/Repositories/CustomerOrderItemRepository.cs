using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Deposit.Data.Repositories
{
    public class CustomerOrderItemRepository : IRepository<CustomerOrderItem>
    {
        private readonly DepositDbContext _context;

        public CustomerOrderItemRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(CustomerOrderItem customerOrderItem)
        {
            _context.CustomerOrderItems.Add(customerOrderItem);

            foreach (var customerDeposit in customerOrderItem.Deposits)
                _context.CustomerDeposits.Add(customerDeposit);

            _context.SaveChanges();
        }

        public IEnumerable<CustomerOrderItem> ListAll()
        {
            return _context.CustomerOrderItems.AsEnumerable();
        }

        public void Update(CustomerOrderItem entity)
        {
            _context.CustomerOrderItems.Update(entity);
            _context.SaveChanges();
            // @TODO ver o que acontece quando tem um deposit novo
        }

        public void Delete(Guid guid)
        {
            var customerItemOrder = ListAll().FirstOrDefault(i => i.Id == guid);

            customerItemOrder.Delete();
            Update(customerItemOrder);
        }
    }
}
