using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Models;
using Deposit.Views;

namespace Deposit.Services
{
    public class CustomerServices
    {
        public List<CustomerView> GetAllCustomers(IRepository<Customer> repository)
        {
            var customers = repository.ReadAll();
            var customersView = new List<CustomerView>();
            
            foreach (var i in customers)
                customersView.Add(new CustomerView()
                {
                    BirthDate = i.BirthDate.ToShortDateString(),
                    Cpf = i.Cpf,
                    Id = i.Id,
                    Name = i.Name,
                    TotalSpent = i.TotalSpent
                });

            return customersView;
        }

        public CustomerView GetCustomer(IRepository<Customer> repository, Guid id)
        {
            var customer = repository.Read(id);
            return new CustomerView()
            {
                Name = customer.Name,
                BirthDate = customer.BirthDate.ToShortDateString(),
                Cpf = customer.Cpf,
                TotalSpent = customer.TotalSpent,
                Id = customer.Id
            };
        }

        public CustomerView CreateCustomer(IRepository<Customer> repository, string name, string cpf, DateTime birthDate)
        {
            var customer = Customer.MakeCustomer(name, cpf, birthDate);
            repository.Add(customer);
            return new CustomerView()
            {
                BirthDate = customer.BirthDate.ToShortDateString(),
                Cpf = customer.Cpf,
                Name = customer.Name,
                TotalSpent = customer.TotalSpent,
                Id = customer.Id
            };
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
