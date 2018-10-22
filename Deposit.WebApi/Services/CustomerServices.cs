using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Services
{
    public class CustomerServices
    {
        public List<CustomerView> GetAllCustomers(IRepository<Customer> repository)
        {
            var customers = repository.ListAll();

            return customers.Where(c => !c.IsDeleted).
                Select(i => new CustomerView()
                {
                    BirthDate = i.BirthDate.ToShortDateString(),
                    Cpf = i.Cpf,
                    Id = i.Id,
                    Name = i.Name,
                    TotalSpent = i.TotalSpent
                })
                .ToList();
        }

        public List<CustomerView> GetTopCustomers(IRepository<Customer> repository)
        {
            var customers = repository.ListAll().OrderByDescending(c => c.TotalSpent).ToList();
            var customersView = new List<CustomerView>();
            var total = (customers.Count < 10) ? customers.Count : 10; 
            
            for (var i = 0; i < total ; i++)
                customersView.Add(new CustomerView()
                {
                    BirthDate = customers[i].BirthDate.ToShortDateString(),
                    Cpf = customers[i].Cpf,
                    Id = customers[i].Id,
                    Name = customers[i].Name,
                    TotalSpent = customers[i].TotalSpent
                });

            return customersView;
        }
        
        public List<CustomerWithOrdersView> GetAllOrders(IRepository<Customer> repository, IRepository<CustomerOrder> orderRepository)
        {
            var customers = repository.ListAll();
            var customersView = new List<CustomerWithOrdersView>();

            foreach (var i in customers)
            {
                var orders = orderRepository.ListAll();
                var ordersView = orders.Where(ord => ord.CustomerId == i.Id).
                    Select(o => new CustomerWithOrdersView.SimpleCustomerOrderView()
                    {
                        Id = o.Id, 
                        RegisterDate = o.RegisterDate.ToShortDateString(), 
                        RegisterNumber = o.RegisterNumber, 
                        TotalValue = o.TotalValue
                    }).ToList();
                
                customersView.Add(new CustomerWithOrdersView()
                {
                    Cpf = i.Cpf,
                    Id = i.Id,
                    Name = i.Name,
                    Orders = ordersView
                });
            }

            return customersView;
        }

        public CustomerView GetCustomer(IRepository<Customer> repository, Guid id)
        {
            var customer = repository.ListAll().FirstOrDefault(c => c.Id == id);

            if (customer.IsDeleted)
                throw new InvalidOperationException("Customer deleted.");

            if (customer == null)
                return null;
            
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
            var customer = repository.ListAll().FirstOrDefault(c => c.Id == id);

            if (customer == null)
                throw new ArgumentException("Customer not found.");

            customer.UpdateDocumentation(name, cpf);
            repository.Update(customer);
        }
    }
}
