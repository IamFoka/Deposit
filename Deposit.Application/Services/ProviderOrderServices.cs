using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Deposit.Application.Services
{
    public class ProviderOrderServices
    {
        private readonly IRepository<ProviderOrder> _repository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProviderOrderItem> _itemsRepository;

        public ProviderOrderServices(IRepository<ProviderOrder> repository, IRepository<ProviderOrderItem> itemsRepository,
            IRepository<Provider> providerRepository, IRepository<Product> productRepository)
        {
            _repository = repository;
            _providerRepository = providerRepository;
            _productRepository = productRepository;
            _itemsRepository = itemsRepository;
        }

        public List<ProviderOrderView> GetAllOrders()
        {
            return _repository.
                FindBy(o => !o.IsDeleted).
                Select(i => new ProviderOrderView()
                {
                    Id = i.Id,
                    Provider = new ProviderView() { Cnpj = i.Provider.Cnpj, Id = i.Provider.Id, Name = i.Provider.Name },
                    RegisterDate = i.RegisterDate.ToShortDateString(),
                    RegisterNumber = i.RegisterNumber,
                    TotalValue = i.TotalValue
                }).
                 ToList();
        }

        public List<ProviderOrderView> GetAllOrders(Guid providerId)
        {
            return _repository.
            FindBy(o => o.ProviderId == providerId && !o.IsDeleted, p => p.Provider)
            .AsEnumerable()
            .Select(i => new ProviderOrderView()
            {
                Id = i.Id,
                Provider = new ProviderView() { Cnpj = i.Provider.Cnpj, Id = i.Provider.Id, Name = i.Provider.Name },
                RegisterDate = i.RegisterDate.ToShortDateString(),
                RegisterNumber = i.RegisterNumber,
                TotalValue = i.TotalValue
            }).ToList();
        }

        public ProviderOrderCompleteView GetOrder(Guid id)
        {
            var order = _repository.GetById(id, o => o.Provider);
            var orderItens = _itemsRepository
                .FindBy(o => o.ProviderOrderId
                .Equals(order.Id), i => i.Product).AsEnumerable();
            return new ProviderOrderCompleteView()
            {
                Id = order.Id,
                RegisterDate = order.RegisterDate.ToShortDateString(),
                RegisterNumber = order.RegisterNumber,
                TotalValue = order.TotalValue,
                Provider = new ProviderView()
                {
                    Cnpj = order.Provider.Cnpj,
                    Id = order.Provider.Id,
                    Name = order.Provider.Name,
                },  
                ProviderOrderItems = orderItens
                ?.Select(i => new ProviderOrderItemView()
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

        public Guid CreateOrder(ProviderOrderDto dto)
        {
            var provider = _providerRepository.GetById(dto.ProviderId);

            if (provider == null)
                throw new ArgumentException("Provider not found.");

            var order = ProviderOrder.MakeProviderOrder(dto.RegisterNumber, provider);

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

        public Guid AddItem(Guid orderId,
            ProviderOrderItemDto dto)
        {
            var order = _repository.GetById(orderId);

            if (order == null)
                throw new ArgumentException("Order not found.");

            var product = _productRepository.GetById(dto.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found.");

            Guid id = order.AddItem(product, dto.Amount);

            _repository.Update(order);

            return id;
        }

        public IEnumerable<Guid> AddItem(Guid orderId,
             IEnumerable<ProviderOrderItemDto> dtos)
        {
            return dtos.Select(i => AddItem(orderId, i));
        }
        public void DeleteProviderOrder(Guid id)
        {
            _repository.Delete(id);
        }
    }
}