using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Views;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Services
{
    public class ProviderServices
    {
        public List<ProviderView> GetAllProviders(IRepository<Provider> repository)
        {
            var providers = repository.ReadAll();
            var providersView = new List<ProviderView>();
            
            foreach (var i in providers)
                providersView.Add(new ProviderView()
                {
                    Id = i.Id,
                    Cnpj = i.Cnpj,
                    Name = i.Name
                });

            return providersView;
        }

        public List<ProviderWithOrdersView> GetAllOrders(IRepository<Provider> repository, IRepository<ProviderOrder> orderRepository)
        {
            var providers = repository.ReadAll();
            var providersView = new List<ProviderWithOrdersView>();

            foreach (var i in providers)
            {
                var orders = orderRepository.ReadAll();
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

        public ProviderView GetProvider(IRepository<Provider> repository, Guid id)
        {
            var provider = repository.Read(id);
            return new ProviderView()
            {
                Id = provider.Id,
                Cnpj = provider.Cnpj,
                Name = provider.Name
            };
        }

        public ProviderView CreateProvider(IRepository<Provider> repository, ProviderDto providerDto)
        {
            var provider = Provider.MakeProvider(providerDto.Name, providerDto.Cnpj);
            repository.Add(provider);
            return new ProviderView()
            {
                Id = provider.Id,
                Cnpj = provider.Cnpj,
                Name = provider.Name
            };
        }

        public void DeleteProvider(IRepository<Provider> repository, Guid id)
        {
            repository.Delete(id);
        }

        public void UpdateProvider(IRepository<Provider> repository, Guid id, ProviderDto providerDto)
        {
            var provider = repository.Read(id);

            if (provider == null)
                throw new ArgumentException("Customer not found.");
            
            provider.UpdateDocumentation(providerDto.Name, providerDto.Cnpj);
            repository.Update(id, provider);
        }
    }
}