using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class ProviderOrderRepository : IRepository<ProviderOrder>
    {
        private static List<ProviderOrder> ProviderOrders { get; set; }

        public ProviderOrderRepository()
        {
            if (ProviderOrders == null)
                ProviderOrders = new List<ProviderOrder>();
        }

        public void Add(ProviderOrder providerOrder)
        {
            ProviderOrders.Add(providerOrder);

            if (providerOrder.ProviderOrderItems.Count != 0)
            {
                var providerOrderItemRepository = new ProviderOrderItemRepository();

                foreach (var providerOrderItem in providerOrder.ProviderOrderItems)
                    providerOrderItemRepository.Add(providerOrderItem);
            }
        }

        public ProviderOrder Read(Guid guid)
        {
            return ProviderOrders.FirstOrDefault(c => c.Id == guid);
        }

        public void Update(Guid guid, ProviderOrder t)
        {

        }

        public void Delete(Guid guid)
        {
            var providerOrder = Read(guid);

            providerOrder.Delete();
        }

        public List<ProviderOrder> ReadAll()
        {
            return new List<ProviderOrder>(ProviderOrders);
        }
    }
}
