using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;

namespace Deposit.Services
{
    public class ProviderOrderServices
    {
        public List<ProviderOrder> GetAllOrders(IRepository<ProviderOrder> repository)
        {
            return repository.ReadAll();
        }

        public ProviderOrder GetOrder(IRepository<ProviderOrder> repository, Guid id)
        {
            return repository.Read(id);
        }

        public ProviderOrder CreateOrder(IRepository<ProviderOrder> repository, 
            IRepository<Provider> providerRepository, ProviderOrderDto dto)
        {
            var provider = providerRepository.Read(dto.ProviderId);
            
            if (provider == null)
                throw new ArgumentException("Provider not found.");

            var order = ProviderOrder.MakeProviderOrder(dto.RegisterNumber, provider);
            
            repository.Add(order);
            return order;
        }

        public void DeleteProviderOrder(IRepository<ProviderOrder> repository, Guid id)
        {
            repository.Delete(id);
        }        
    }
}