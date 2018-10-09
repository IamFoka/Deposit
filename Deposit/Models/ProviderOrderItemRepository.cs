using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
{
    public class ProviderOrderItemRepository : IRepository<ProviderOrderItem>
    {
        private static List<ProviderOrderItem> ProviderOrderItems { get; set; }
        private static List<ProviderDeposit> ProviderDeposits { get; set; }

        public ProviderOrderItemRepository()
        {
            if (ProviderOrderItems == null)
                ProviderOrderItems = new List<ProviderOrderItem>();

            if (ProviderDeposits == null)
                ProviderDeposits = new List<ProviderDeposit>();
        }

        public void Add(ProviderOrderItem providerOrderItem)
        {
            ProviderOrderItems.Add(providerOrderItem);

            foreach (var providerDeposit in providerOrderItem.Deposits)
                ProviderDeposits.Add(providerDeposit);
        }

        public ProviderOrderItem Read(Guid guid)
        {
            return ProviderOrderItems.FirstOrDefault(c => c.Id == guid);
        }

        public void Update(Guid guid, ProviderOrderItem t)
        {

        }

        public void Delete(Guid guid)
        {
            var providerOrderItem = Read(guid);

            providerOrderItem.Delete();
        }

        public List<ProviderOrderItem> ReadAll()
        {
            return new List<ProviderOrderItem>(ProviderOrderItems);
        }
    }
}
