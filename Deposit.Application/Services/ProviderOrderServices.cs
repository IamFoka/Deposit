using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class ProviderOrderServices
    {
        private readonly IRepository<ProviderOrder> _repository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Product> _productRepository;

        public ProviderOrderServices(IRepository<ProviderOrder> repository, IRepository<Provider> providerRepository, IRepository<Product> productRepository)
        {
            _repository = repository;
            _providerRepository = providerRepository;
            _productRepository = productRepository;
        }

        public List<ProviderOrderView> GetAllOrders()
        {   
            var orders = _repository.ListAll();

            if (orders == null)
                return null;    

            return orders.Select(i => new ProviderOrderView()
                {
                    Id = i.Id,
                    Provider = new ProviderView() {Cnpj = i.Provider.Cnpj, Id = i.Provider.Id, Name = i.Provider.Name},
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                }).ToList();  
        }
        
        public List<ProviderOrderView> GetAllOrders(Guid providerId)
        {
            var orders = _repository.ListAll();

            return orders.Where(ord => ord.ProviderId == providerId)
                .Select(i => new ProviderOrderView()
                {
                    Id = i.Id,
                    Provider = new ProviderView() {Cnpj = i.Provider.Cnpj, Id = i.Provider.Id, Name = i.Provider.Name},
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                }).ToList();
        }

        public ProviderOrderCompleteView GetOrder(Guid id)
        {
            var order = _repository.ListAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
                return null;

            if (order.ProviderOrderItems == null)
                return null;

            var items = order.ProviderOrderItems
                .Select(i => new ProviderOrderItemView()
                {
                    Amount = i.Amount,
                    Id = i.Id,
                    Price = i.Price,
                    Product = i.Product.Name,
                    ProductId = i.Product.Id,
                    TotalValue = i.TotalValue
                }).ToList();
            
            return new ProviderOrderCompleteView()
            {
                Id = order.Id,
                Provider = new ProviderView()
                {
                    Cnpj = order.Provider.Cnpj,
                    Id = order.Provider.Id,
                    Name = order.Provider.Name
                },
                ProviderOrderItems = items,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue
            };
        }

        public ProviderOrderCompleteView CreateOrder(ProviderOrderDto dto)
        {
            var provider = _providerRepository.ListAll().FirstOrDefault(p => p.Id == dto.ProviderId);
            
            if (provider == null)
                throw new ArgumentException("Provider not found.");

            var order = ProviderOrder.MakeProviderOrder(dto.RegisterNumber, provider);

            foreach (var i in dto.Items)
            {
                var product = _productRepository.ListAll().FirstOrDefault(p => p.Id == i.ProductId);
                
                if (product == null)
                    throw new ArgumentException("Product not found.");
                
                order.AddItem(product, i.Amount);
            }
            
            _repository.Add(order);

            var items = order.ProviderOrderItems
                .Select(i => new ProviderOrderItemView()
                {
                    Amount = i.Amount,
                    Id = i.Id,
                    Price = i.Price,
                    Product = i.Product.Name,
                    ProductId = i.Product.Id,
                    TotalValue = i.TotalValue
                }).ToList();

            return new ProviderOrderCompleteView()
            {
                Id = order.Id,
                Provider = new ProviderView()
                {
                    Cnpj = order.Provider.Cnpj,
                    Id = order.Provider.Id,
                    Name = order.Provider.Name
                },
                ProviderOrderItems = items,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue
            };
        }

        public ProviderOrderItemView AddItem(Guid id,  
            ProviderOrderItemDto dto)
        {
            var order = _repository.ListAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
                throw new ArgumentException("Order not found.");

            var product = _productRepository.ListAll().FirstOrDefault(p => p.Id == dto.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found.");

            ProviderOrderItem item = order.AddItem(product, dto.Amount);

            _repository.Update(order);

            return new ProviderOrderItemView()
                {
                Id = item.Id,
                ProductId = item.ProductId,
                Product = item.Product.Name,
                Amount = item.Amount,
                Price = item.Price,
                TotalValue= item.TotalValue
                };

                
        }

        public void DeleteProviderOrder(Guid id)
        {   
            _repository.Delete(id);
        }        
    }
}