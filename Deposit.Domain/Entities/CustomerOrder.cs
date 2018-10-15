using System;
using System.Collections.Generic;

namespace Deposit.Domain.Entities
{
    public class CustomerOrder : Entity
    {
        public DateTime RegisterDate { get; private set; }
        public int RegisterNumber { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public float TotalValue { get; private set; }
        public List<CustomerOrderItem> CustomerOrderItems { get; private set; }

        protected CustomerOrder() :
            base()
        { }

        public static CustomerOrder MakeCustomerOrder(int registerNumber, Customer customer)
        {
            if (registerNumber <= 0)
                throw new ArgumentException("Register number must be larger than 0.");

            if (customer == null)
                throw new ArgumentNullException("Customer must have a value.");

            var customerOrder = new CustomerOrder();

            customerOrder.RegisterDate = DateTime.Now;
            customerOrder.RegisterNumber = registerNumber;
            customerOrder.CustomerId = customer.Id;
            customerOrder.Customer = customer;
            customerOrder.TotalValue = 0;
            customerOrder.CustomerOrderItems = new List<CustomerOrderItem>();

            return customerOrder;
        }

        public void AddItem(Product product, float amount)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order is deleted.");

            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(this, product, amount);
            CustomerOrderItems.Add(customerOrderItem);
        }

        public void UpdateTotalValue(float value)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order is deleted.");

            if (value == 0)
                throw new ArgumentException("Value can't be equal to zero 0.");

            if (TotalValue + value < 0)
                throw new ArgumentException("Total value can't be lower than 0.");

            TotalValue += value;
            Customer.UpdateTotalSpent(value);
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order is already deleted.");

            foreach (var item in CustomerOrderItems)
                item.Delete();

            base.Delete();
        }
    }
}