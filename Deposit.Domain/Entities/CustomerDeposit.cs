using System;

namespace Deposit.Domain.Entities
{
    public class CustomerDeposit : Entity
    {
        public DateTime RegisterDate { get; protected set; }
        public DepositMovement MovementType { get; protected set; }
        public float Amount { get; protected set; }
        public Guid CustomerOrderItemId { get; protected set; }
        public CustomerOrderItem CustomerOrderItem { get; protected set; }
        public string Sku { get; protected set; }

        protected CustomerDeposit() :
            base()
        { }

        public static CustomerDeposit MakeCustomerDeposit(CustomerOrderItem customerOrderItem, float amount, DepositMovement depositMovement)
        {
            if (customerOrderItem == null)
                throw new ArgumentNullException("Customer order item must have a value.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            var deposit = new CustomerDeposit();

            deposit.Amount = amount;

            deposit.RegisterDate = DateTime.Now;
            deposit.MovementType = depositMovement;
            deposit.CustomerOrderItemId = customerOrderItem.Id;
            deposit.CustomerOrderItem = customerOrderItem;
            deposit.Sku = customerOrderItem.Product.Sku;

            deposit.CustomerOrderItem.Product.UpdateAmount(depositMovement == DepositMovement.In ? amount : -amount);

            return deposit;
        }

        public override void Delete()
        {
            throw new InvalidOperationException("A deposit movement can't be deleted.");
        }
    }
}