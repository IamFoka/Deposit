using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;

namespace Deposit.Services
{
    public class ProviderServices
    {
        public List<Provider> GetAllProviders(IRepository<Provider> repository)
        {
            return repository.ReadAll();
        }

        public Provider GetProvider(IRepository<Provider> repository, Guid id)
        {
            return repository.Read(id);
        }

        public Provider CreateProvider(IRepository<Provider> repository, ProviderDto providerDto)
        {
            var provider = Provider.MakeProvider(providerDto.Name, providerDto.Cnpj);
            repository.Add(provider);
            return provider;
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