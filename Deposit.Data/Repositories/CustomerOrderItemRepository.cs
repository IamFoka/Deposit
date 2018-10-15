using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class CustomerOrderItemRepository : IRepository<CustomerOrderItem>
    {
        private static List<CustomerOrderItem> CustomerOrderItems { get; set; }
        private static List<CustomerDeposit> CustomerDeposits { get; set; }

        public CustomerOrderItemRepository()
        {
            if (CustomerOrderItems == null)
                CustomerOrderItems = new List<CustomerOrderItem>();

            if (CustomerDeposits == null)
                CustomerDeposits = new List<CustomerDeposit>();
        }

        public void Add(CustomerOrderItem customerOrderItem)
        {
            CustomerOrderItems.Add(customerOrderItem);

            foreach (var customerDeposit in customerOrderItem.Deposits)
                CustomerDeposits.Add(customerDeposit);
        }

        public CustomerOrderItem Read(Guid guid)
        {
            return CustomerOrderItems.FirstOrDefault(c => c.Id == guid);
        }

        public void Update(Guid guid, CustomerOrderItem t)
        {

        }

        public void Delete(Guid guid)
        {
            var customerItemOrder = Read(guid);

            customerItemOrder.Delete();
        }

        public List<CustomerOrderItem> ReadAll()
        {
            return new List<CustomerOrderItem>(CustomerOrderItems);
        }
    }
}
