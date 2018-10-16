using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly DepositDbContext _context;

        public CustomerRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public IEnumerable<Customer> ListAll()
        {
            return _context.Customers.AsEnumerable();
        }

        public void Update(Customer entity)
        {
            _context.Customers.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var customer = ListAll().FirstOrDefault(p => p.Id == guid);

            if (customer == null)
                throw new ArgumentException("Customer not found.");

            customer.Delete();
            Update(customer);
        }
    }
}
