using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
{
    public class ProductRepository : IRepository<Product>
    {
        private static List<Product> Products { get; set; }

        public ProductRepository()
        {
            if (Products == null)
                Products = new List<Product>();
        }

        public void Add(Product product)
        {
            Products.Add(product);
        }

        public Product Read(Guid guid)
        {
            return Products.FirstOrDefault(c => c.Id == guid);
        }

        public void Update(Guid guid, Product t)
        {

        }

        public void Delete(Guid guid)
        {
            var product = Read(guid);
            product.Delete();
        }

        public List<Product> ReadAll()
        {
            return new List<Product>(Products);
        }
    }
}
