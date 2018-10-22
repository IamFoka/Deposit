using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Deposit.Data.Repositories
{
    public class CustomerOrderRepository : IRepository<CustomerOrder>
    {
        private readonly DepositDbContext _context;

        public CustomerOrderRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(CustomerOrder customerOrder)
        {
            _context.CustomerOrders.Add(customerOrder);
            _context.SaveChanges();

            if (customerOrder.CustomerOrderItems.Count == 0)
                return;

            var customerOrderItemRepository = new CustomerOrderItemRepository(_context);

            foreach (var customerOrderItem in customerOrder.CustomerOrderItems)
                customerOrderItemRepository.Add(customerOrderItem);
        }

        public IEnumerable<CustomerOrder> ListAll()
        {
            return _context.CustomerOrders.Include(o => o.Customer).AsEnumerable();
        }

        public void Update(CustomerOrder entity)
        {
            _context.CustomerOrders.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var customerOrder = ListAll().FirstOrDefault(o => o.Id == guid);

            if (customerOrder == null)
                throw new ArgumentException("Customer order not found.");

            customerOrder.Delete();
            Update(customerOrder);
        }
    }
}
