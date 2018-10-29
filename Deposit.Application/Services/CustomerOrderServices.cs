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

        private readonly IRepository<CustomerOrderItem> _itemsRepository;

        public CustomerOrderServices(IRepository<CustomerOrder> repository, IRepository<Product> productRepository, IRepository<Customer> customerRepository,
        IRepository<CustomerOrderItem> itemsRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _itemsRepository = itemsRepository;
        }

        public List<CustomerOrderView> GetAllOrders()
        {
            return _repository
                .FindBy(o => !o.IsDeleted, o => o.Customer, o => o.Customer)
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

        public List<CustomerOrderView> GetAllOrders(Guid customerId)
        {
            return _repository
                .FindBy(o => o.Id.Equals(customerId) && !o.IsDeleted)
                .Select(i => new CustomerOrderView()
                {
                    Id = i.Id,
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue,
                    Customer = new CustomerView()
                    {
                        BirthDate = i.Customer.BirthDate.ToShortDateString(),
                        Cpf = i.Customer.Cpf,
                        Id = i.CustomerId,
                        Name = i.Customer.Name,
                        TotalSpent = i.Customer.TotalSpent
                    }
                })
                .ToList();
        }

        public CustomerOrderCompleteView GetOrder(Guid id)
        {
            var order = _repository.GetById(id, o => o.Customer);
            var orderItens = _itemsRepository
                .FindBy(o => o.CustomerOrderId
                .Equals(order.Id), i => i.Product).AsEnumerable();
            return new CustomerOrderCompleteView()
            {
                Id = order.Id,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue,
                Customer = new CustomerView()
                {
                    Cpf = order.Customer.Cpf,
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    BirthDate = order.Customer.BirthDate.ToShortDateString(),
                    TotalSpent = order.TotalValue
                },  
                CustomerOrderItems = orderItens
                ?.Select(i => new CustomerOrderItemView()
                {
                    Amount = i.Amount,
                    Id = i.Id,
                    Price = i.Price,
                    Product = i.Product.Name,
                    ProductId = i.Product.Id,
                    TotalValue = i.TotalValue
                }).ToList()
            };
        }


        public Guid CreateOrder(CustomerOrderDto dto)
        {
            var customer = _customerRepository.GetById(dto.CustomerId);

            if (customer == null)
                throw new ArgumentException("Customer not found.");

            var order = CustomerOrder.MakeCustomerOrder(dto.RegisterNumber, customer);

            var products = _productRepository
            .FindBy(p => dto.Items
            .Select(r => r.ProductId)
            .Contains(p.Id))
            .AsEnumerable();

            dto.Items
            .ForEach(i => order.AddItem(products.FirstOrDefault(p => p.Id.Equals(i.ProductId)), i.Amount));

            _repository.Add(order);

            return order.Id;
        }

        public CustomerOrderItemView AddItem(Guid id,
            CustomerOrderItemDto dto)
        {
            var order = _repository.GetAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
                throw new ArgumentException("Order not found.");

            var product = _productRepository.GetAll().FirstOrDefault(p => p.Id == dto.ProductId);

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
                TotalValue = item.TotalValue
            };
        }

        public void DeleteCustomerOrder(Guid id)
        {
            _repository.Delete(id);
        }
    }
}