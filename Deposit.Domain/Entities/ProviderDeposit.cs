using System;

namespace Deposit.Domain.Entities
{
    public class ProviderDeposit : Entity
    {
        public DateTime RegisterDate { get; protected set; }
        public DepositMovement MovementType { get; protected set; }
        public float Amount { get; protected set; }
        public Guid ProviderOrderItemId { get; protected set; }
        public ProviderOrderItem ProviderOrderItem { get; protected set; }
        public string Sku { get; protected set; }

        protected ProviderDeposit() :
            base()
        { }

        public static ProviderDeposit MakeProviderDeposit(ProviderOrderItem providerOrderItem, float amount, DepositMovement depositMovement)
        {
            if (providerOrderItem == null)
                throw new ArgumentNullException("Provider order item must have a value.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            var deposit = new ProviderDeposit();

            deposit.Amount = amount;

            deposit.RegisterDate = DateTime.Now;
            deposit.MovementType = depositMovement;
            deposit.ProviderOrderItemId = providerOrderItem.Id;
            deposit.ProviderOrderItem = providerOrderItem;
            deposit.Sku = providerOrderItem.Product.Sku;

            deposit.ProviderOrderItem.Product.UpdateAmount(depositMovement == DepositMovement.In ? amount : -amount);

            return deposit;
        }

        public override void Delete()
        {
            throw new InvalidOperationException("A deposit movement can't be deleted.");
        }
    }
}