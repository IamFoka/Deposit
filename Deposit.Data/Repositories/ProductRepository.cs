using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly DepositDbContext _context;

        public ProductRepository(DepositDbContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> ListAll()
        {
            return _context.Products.AsEnumerable();
        }

        public void Update(Product entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid guid)
        {
            var product = ListAll().FirstOrDefault(p => p.Id == guid);

            if (product == null)
                throw new ArgumentException("Product not found.");
            
            product.Delete();
            Update(product);
        }
    }
}
