using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Application.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class ProviderServices
    {
        private readonly IRepository<Provider> _repository;
        private readonly IRepository<ProviderOrder> _orderRepository;

        public ProviderServices(IRepository<Provider> repository, IRepository<ProviderOrder> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }

        public List<ProviderView> GetAllProviders()
        {
            var providers = _repository.ListAll();

            return providers.Where(p => !p.IsDeleted).
            Select(i => new ProviderView()
            {
                Id = i.Id,
                Cnpj = i.Cnpj,
                Name = i.Name
            }).ToList();
        }

        public List<ProviderWithOrdersView> GetAllOrders()
        {
            var providers = _repository.ListAll();
            var providersView = new List<ProviderWithOrdersView>();

            foreach (var i in providers)
            {
                var orders = _orderRepository.ListAll();
                var ordersView = orders.Where(ord => ord.ProviderId == i.Id).
                    Select(o => new ProviderWithOrdersView.SimpleProviderOrderView()
                    {
                        Id = o.Id, 
                        RegisterDate = o.RegisterDate.ToShortDateString(), 
                        RegisterNumber = o.RegisterNumber, 
                        TotalValue = o.TotalValue
                    }).ToList();
                
                providersView.Add(new ProviderWithOrdersView()
                {
                    Cnpj = i.Cnpj,
                    Id = i.Id,
                    Name = i.Name,
                    Orders = ordersView
                });
            }

            return providersView;
        }

        public ProviderView GetProvider(Guid id)
        {
            var provider = _repository.ListAll().FirstOrDefault(p => p.Id == id);

            if (provider.IsDeleted)
            {
                throw new InvalidOperationException("Provider deleted.");
            }

            if (provider == null)
                return null;
            
            return new ProviderView()
            {
                Id = provider.Id,
                Cnpj = provider.Cnpj,
                Name = provider.Name
            };
        }

        public ProviderView CreateProvider(ProviderDto providerDto)
        {
            var provider = Provider.MakeProvider(providerDto.Name, providerDto.Cnpj);
            _repository.Add(provider);
            return new ProviderView()
            {
                Id = provider.Id,
                Cnpj = provider.Cnpj,
                Name = provider.Name
            };
        }

        public void DeleteProvider(Guid id)
        {
            _repository.Delete(id);
        }

        public void UpdateProvider(Guid id, ProviderDto providerDto)
        {
            var provider = _repository.ListAll().FirstOrDefault(p => p.Id == id);

            if (provider == null)
                throw new ArgumentException("Customer not found.");
            
            provider.UpdateDocumentation(providerDto.Name, providerDto.Cnpj);
            _repository.Update(provider);
        }
    }
}