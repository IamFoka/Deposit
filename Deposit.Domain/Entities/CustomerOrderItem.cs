using System;
using System.Collections.Generic;

namespace Deposit.Domain.Entities
{
    public class CustomerOrderItem : Entity
    {
        public Guid CustomerOrderId { get; protected set; }
        public CustomerOrder CustomerOrder { get; protected set; }
        public Guid ProductId { get; protected set; }
        public Product Product { get; protected set; }
        public float Amount { get; protected set; }
        public float Price { get; protected set; }
        public float TotalValue { get; protected set; }
        public List<CustomerDeposit> Deposits { get; protected set; }

        private CustomerOrderItem() :
            base()
        { }

        public static CustomerOrderItem MakeCustomerOrderItem(CustomerOrder customerOrder, Product product, float amount)
        {
            if (customerOrder == null)
                throw new ArgumentNullException("Customer order must have a value.");

            if (product == null)
                throw new ArgumentNullException("Product must have a value.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            if (product.Amount < amount)
                throw new ArgumentException("Amount larger than product deposit.");

            var customerOrderItem = new CustomerOrderItem();

            customerOrderItem.CustomerOrderId = customerOrder.Id;
            customerOrderItem.CustomerOrder = customerOrder;
            customerOrderItem.ProductId = product.Id;
            customerOrderItem.Product = product;
            customerOrderItem.Amount = amount;
            customerOrderItem.Price = product.Price;
            customerOrderItem.TotalValue = customerOrderItem.Price * customerOrderItem.Amount;
            customerOrderItem.Deposits = new List<CustomerDeposit>();

            customerOrder.UpdateTotalValue(customerOrderItem.TotalValue);

            var deposit = CustomerDeposit.MakeCustomerDeposit(customerOrderItem, amount, DepositMovement.Out);
            customerOrderItem.Deposits.Add(deposit);

            return customerOrderItem;
        }

        public void ChangeAmount(float amount)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is deleted.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            if (Amount == amount)
                throw new ArgumentException("Amount can't be the same as before.");

            if (Product.Amount < amount)
                throw new ArgumentException("Amount larger than product deposit.");

            var difference = amount - Amount;
            var movementType = DepositMovement.Out;

            if (difference < 0)
            {
                difference = -difference;
                movementType = DepositMovement.In;
            }

            Amount = amount;
            var oldValue = TotalValue;
            TotalValue = Price * Amount;

            var deposit = CustomerDeposit.MakeCustomerDeposit(this, difference, movementType);
            Deposits.Add(deposit);

            difference = TotalValue - oldValue;
            CustomerOrder.UpdateTotalValue(difference);
        }

        public void ChangePrice(float price)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is deleted.");

            if (price <= 0)
                throw new ArgumentException("Price must be larger than 0.");

            if (price == Price)
                throw new ArgumentException("Price can't be the same as before.");

            Price = price;
            var oldValue = TotalValue;
            TotalValue = Price * Amount;

            var difference = TotalValue - oldValue;
            CustomerOrder.UpdateTotalValue(difference);
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is already deleted.");

            var deletedMovementItem = CustomerDeposit.MakeCustomerDeposit(this, Amount, DepositMovement.In);
            Deposits.Add(deletedMovementItem);

            CustomerOrder.Customer.UpdateTotalSpent(-Amount);

            base.Delete();
        }
    }
}