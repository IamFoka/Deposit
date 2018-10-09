using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
{
    public class CustomerRepository : IRepository<Customer>
    {
        private static List<Customer> Customers { get; set; }

        public CustomerRepository()
        {
            if (Customers == null)
                Customers = new List<Customer>();
        }

        public void Add(Customer customer)
        {
            Customers.Add(customer);
        }

        public Customer Read(Guid guid)
        {
            return Customers.FirstOrDefault(c => c.Id == guid && !c.IsDeleted);
        }

        public List<Customer> ReadAll()
        {
            var l = new List<Customer>();

            foreach (var i in Customers)
                if (!i.IsDeleted)
                    l.Add(i);

            return l;
        }

        public void Update(Guid guid, Customer t)
        {

        }

        public void Delete(Guid guid)
        {
            var customer = Read(guid);

            if (customer == null)
                throw new ArgumentException();

            customer.Delete();
        }
    }
}
