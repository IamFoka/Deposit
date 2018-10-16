using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Services
{
    public class CustomerOrderServices
    {
        public List<CustomerOrderView> GetAllOrders(IRepository<CustomerOrder> repository)
        {
            var orders = repository.ListAll();

            return orders.Select(i => new CustomerOrderView()
                {
                    Customer = new CustomerView()
                    {
                        BirthDate = i.Customer.BirthDate.ToShortDateString(),
                        Cpf = i.Customer.Cpf,
                        Id = i.CustomerId,
                        Name = i.Customer.Name,
                        TotalSpent = i.Customer.TotalSpent
                    },
                    Id = i.Id,
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                })
                .ToList();
        }
        
        public List<CustomerOrderView> GetAllOrders(IRepository<CustomerOrder> repository, Guid customerId)
        {
            var orders = repository.ListAll();

            return orders.Where(ord => ord.CustomerId == customerId)
                .Select(i => new CustomerOrderView()
                {
                    Customer = new CustomerView()
                    {
                        BirthDate = i.Customer.BirthDate.ToShortDateString(),
                        Cpf = i.Customer.Cpf,
                        Id = i.CustomerId,
                        Name = i.Customer.Name,
                        TotalSpent = i.Customer.TotalSpent
                    },
                    Id = i.Id,
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                })
                .ToList();
        }

        public CustomerOrderCompleteView GetOrder(IRepository<CustomerOrder> repository, Guid id)
        {
            var order = repository.ListAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
                return null;

            var items = order.CustomerOrderItems
                .Select(i => new CustomerOrderItemView()
                {
                    Amount = i.Amount,
                    Id = i.ProductId,
                    Price = i.Price,
                    Product = i.Product.Name,
                    ProductId = i.ProductId,
                    TotalValue = i.TotalValue
                })
                .ToList();
                
            
            return new CustomerOrderCompleteView()
            {
                Customer = new CustomerView()
                {
                    BirthDate = order.Customer.BirthDate.ToShortDateString(),
                    Cpf = order.Customer.Cpf,
                    Id = order.CustomerId,
                    Name = order.Customer.Name,
                    TotalSpent = order.Customer.TotalSpent
                },
                Id = order.Id,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue,
                Items = items
            };
        }

        public CustomerOrderCompleteView CreateOrder(IRepository<CustomerOrder> repository, 
            IRepository<Customer> customerRepository, IRepository<Product> productRepository, CustomerOrderDto dto)
        {
            var customer = customerRepository.ListAll().FirstOrDefault(c => c.Id == dto.CustomerId);
            
            if (customer == null)
                throw new ArgumentException("Customer not found.");

            var order = CustomerOrder.MakeCustomerOrder(dto.RegisterNumber, customer);
            var items = new List<CustomerOrderItemView>();

            foreach (var i in dto.Items)
            {
                var product = productRepository.ListAll().FirstOrDefault(p => p.Id == i.ProductId);
                
                if (product == null)
                    throw new ArgumentException("Product not found.");
                
                order.AddItem(product, i.Amount);
            }
            
            repository.Add(order);
            
            foreach (var i in order.CustomerOrderItems)
                items.Add(new CustomerOrderItemView()
                {
                    Amount = i.Amount,
                    Id = i.Id,
                    Price = i.Price,
                    Product = i.Product.Name,
                    ProductId = i.ProductId,
                    TotalValue = i.TotalValue
                });
                
            return new CustomerOrderCompleteView()
            {
                Customer = new CustomerView()
                {
                    BirthDate = order.Customer.BirthDate.ToShortDateString(),
                    Cpf = order.Customer.Cpf,
                    Id = order.CustomerId,
                    Name = order.Customer.Name,
                    TotalSpent = order.Customer.TotalSpent
                },
                Items = items,
                Id = order.Id,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue
            };
        }

        public void DeleteCustomerOrder(IRepository<CustomerOrder> repository, Guid id)
        {
            repository.Delete(id);
        }
    }
}