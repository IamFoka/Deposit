using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Models;

namespace Deposit.Services
{
    public class CustomerServices
    {
        public List<Customer> GetAllCustomers(IRepository<Customer> repository)
        {
            return repository.ReadAll();
        }

        public Customer GetCustomer(IRepository<Customer> repository, Guid id)
        {
            return repository.Read(id);
        }

        public Customer CreateCustomer(IRepository<Customer> repository, string name, string cpf, DateTime birthDate)
        {
            var customer = Customer.MakeCustomer(name, cpf, birthDate);
            repository.Add(customer);
            return customer;
        }

        public void DeleteCustomer(IRepository<Customer> repository, Guid id)
        {
            repository.Delete(id);
        }

        public void UpdateCustomer(IRepository<Customer> repository, Guid id, string name, string cpf)
        {
            var customer = repository.Read(id);

            if (customer == null)
                throw new ArgumentException("Customer not found.");

            customer.UpdateDocumentation(name, cpf);
            repository.Update(id, customer);
        }
    }
}
