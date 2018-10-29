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
            return _repository.
            FindBy(p => !p.IsDeleted).
            Select(i => new ProviderView()
            {
                Id = i.Id,
                Cnpj = i.Cnpj,
                Name = i.Name
            })
             .ToList();
        }

        public List<ProviderWithOrdersView> GetAllOrders()
        {
            return _repository.GetAll(p => p.ProviderOrders)
            .Select(r => new ProviderWithOrdersView()
            {
                Cnpj = r.Cnpj,
                Id = r.Id,
                Name = r.Name,
                Orders = r.ProviderOrders.Select(p => new ProviderSimpleOrderView()
                {
                    Id = p.Id,
                    RegisterDate = p.RegisterDate.ToShortDateString(),
                    RegisterNumber = p.RegisterNumber,
                    TotalValue = p.TotalValue
                })
            }).ToList();
        }

        public ProviderView GetProvider(Guid id)
        {
            var provider = _repository.GetById(id);

            if (provider.IsDeleted)
                throw new InvalidOperationException("Provider deleted.");

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
            var provider = _repository.GetById(id);

            if (provider == null)
                throw new ArgumentException("Customer not found.");

            provider.UpdateDocumentation(providerDto.Name, providerDto.Cnpj);
            _repository.Update(provider);
        }
    }
}