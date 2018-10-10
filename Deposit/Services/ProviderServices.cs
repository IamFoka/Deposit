using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;
using Deposit.Views;

namespace Deposit.Services
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