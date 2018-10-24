using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class CustomerOrderServices
    {
        private readonly IRepository<CustomerOrder> _repository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerOrderServices(IRepository<CustomerOrder> repository, IRepository<Product> productRepository, IRepository<Customer> customerRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public List<CustomerOrderView> GetAllOrders()
        {
            var orders = _repository.ListAll();

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
        
        public List<CustomerOrderView> GetAllOrders(Guid customerId)
        {
            var orders = _repository.ListAll();

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

        public CustomerOrderCompleteView GetOrder(Guid id)
        {
            var order = _repository.ListAll().FirstOrDefault(o => o.Id == id);

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

        public CustomerOrderCompleteView CreateOrder(CustomerOrderDto dto)
        {
            var customer = _customerRepository.ListAll().FirstOrDefault(c => c.Id == dto.CustomerId);
            
            if (customer == null)
                throw new ArgumentException("Customer not found.");

            var order = CustomerOrder.MakeCustomerOrder(dto.RegisterNumber, customer);
            var items = new List<CustomerOrderItemView>();

            foreach (var i in dto.Items)
            {
                var product = _productRepository.ListAll().FirstOrDefault(p => p.Id == i.ProductId);
                
                if (product == null)
                    throw new ArgumentException("Product not found.");
                
                order.AddItem(product, i.Amount);
            }
            
            _repository.Add(order);
            
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

        public CustomerOrderItemView AddItem(Guid id,
            CustomerOrderItemDto dto)
        {
            var order = _repository.ListAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
                throw new ArgumentException("Order not found.");

            var product = _productRepository.ListAll().FirstOrDefault(p => p.Id == dto.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found.");

            CustomerOrderItem item = order.AddItem(product, dto.Amount);

            _repository.Update(order);

            return new CustomerOrderItemView()
                {
                Id = item.Id,
                ProductId = item.ProductId,
                Product = item.Product.Name,
                Amount = item.Amount,
                Price = item.Price,
                TotalValue= item.TotalValue
                };
        }

        public void DeleteCustomerOrder(Guid id)
        {
            _repository.Delete(id);
        }
    }
}