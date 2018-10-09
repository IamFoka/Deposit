using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
{
    public class CustomerOrderRepository : IRepository<CustomerOrder>
    {
        private static List<CustomerOrder> CustomerOrders { get; set; }

        public CustomerOrderRepository()
        {
            if (CustomerOrders == null)
                CustomerOrders = new List<CustomerOrder>();
        }

        public void Add(CustomerOrder customerOrder)
        {
            CustomerOrders.Add(customerOrder);

            if (customerOrder.CustomerOrderItems.Count != 0)
            {
                var customerOrderItemRepository = new CustomerOrderItemRepository();

                foreach (var customerOrderItem in customerOrder.CustomerOrderItems)
                    customerOrderItemRepository.Add(customerOrderItem);
            }
        }

        public CustomerOrder Read(Guid guid)
        {
            return CustomerOrders.FirstOrDefault(c => c.Id == guid);
        }

        public void Update(Guid guid, CustomerOrder t)
        {

        }

        public void Delete(Guid guid)
        {
            var customerOrder = Read(guid);

            customerOrder.Delete();
        }

        public List<CustomerOrder> ReadAll()
        {
            return new List<CustomerOrder>(CustomerOrders);
        }
    }
}
