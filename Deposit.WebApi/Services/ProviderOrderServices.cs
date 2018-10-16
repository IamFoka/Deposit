using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Services
{
    public class ProviderOrderServices
    {
        public List<ProviderOrderView> GetAllOrders(IRepository<ProviderOrder> repository)
        {
            var orders = repository.ListAll();

            return orders.Select(i => new ProviderOrderView()
                {
                    Id = i.Id,
                    Provider = new ProviderView() {Cnpj = i.Provider.Cnpj, Id = i.Provider.Id, Name = i.Provider.Name},
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                }).ToList();
        }
        
        public List<ProviderOrderView> GetAllOrders(IRepository<ProviderOrder> repository, Guid providerId)
        {
            var orders = repository.ListAll();

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

        public ProviderOrderCompleteView GetOrder(IRepository<ProviderOrder> repository, Guid id)
        {
            var order = repository.ListAll().FirstOrDefault(o => o.Id == id);

            if (order == null)
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

        public ProviderOrderCompleteView CreateOrder(IRepository<ProviderOrder> repository, 
            IRepository<Provider> providerRepository, IRepository<Product> productRepository, ProviderOrderDto dto)
        {
            var provider = providerRepository.ListAll().FirstOrDefault(p => p.Id == dto.ProviderId);
            
            if (provider == null)
                throw new ArgumentException("Provider not found.");

            var order = ProviderOrder.MakeProviderOrder(dto.RegisterNumber, provider);

            foreach (var i in dto.Items)
            {
                var product = productRepository.ListAll().FirstOrDefault(p => p.Id == i.ProductId);
                
                if (product == null)
                    throw new ArgumentException("Product not found.");
                
                order.AddItem(product, i.Amount);
            }
            
            repository.Add(order);

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

        public void DeleteProviderOrder(IRepository<ProviderOrder> repository, Guid id)
        {
            repository.Delete(id);
        }        
    }
}