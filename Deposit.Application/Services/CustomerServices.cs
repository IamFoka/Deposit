using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Application.Services
{
    public class CustomerServices
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<CustomerOrder> _orderRepository;

        public CustomerServices(IRepository<Customer> repository, IRepository<CustomerOrder> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }

        public List<CustomerView> GetAllCustomers()
        {
            return _repository.
                FindBy(c => !c.IsDeleted).
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

        public List<CustomerView> GetTopCustomers()
        {
            var customers = _repository.GetAll().OrderByDescending(c => c.TotalSpent).ToList();
            var customersView = new List<CustomerView>();
            var total = (customers.Count < 10) ? customers.Count : 10;

            for (var i = 0; i < total; i++)
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

        public List<CustomerWithOrdersView> GetAllOrders()
        {
            return _repository.GetAll(c => c.CustomerOrders)
            .Select(r => new CustomerWithOrdersView()
            {
                Cpf = r.Cpf,
                Id = r.Id,
                Name = r.Name,
                Orders = r.CustomerOrders.Select(c => new CustomerSimpleOrderView()
                {
                    Id = c.Id,
                    RegisterDate = c.RegisterDate.ToShortDateString(),
                    RegisterNumber = c.RegisterNumber,
                    TotalValue = c.TotalValue
                })
            }).ToList();
        }
        

        public CustomerView GetCustomer(Guid id)
        {
            var customer = _repository.GetById(id);

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

        public CustomerView CreateCustomer(string name, string cpf, DateTime birthDate)
        {
            var customer = Customer.MakeCustomer(name, cpf, birthDate);
            _repository.Add(customer);
            return new CustomerView()
            {
                BirthDate = customer.BirthDate.ToShortDateString(),
                Cpf = customer.Cpf,
                Name = customer.Name,
                TotalSpent = customer.TotalSpent,
                Id = customer.Id
            };
        }

        public void DeleteCustomer(Guid id)
        {
            _repository.Delete(id);
        }

        public void UpdateCustomer(Guid id, string name, string cpf)
        {
            var customer = _repository.GetById(id);

            if (customer == null)
                throw new ArgumentException("Customer not found.");

            customer.UpdateDocumentation(name, cpf);
            _repository.Update(customer);
        }
    }
}
