using System;
using System.Collections.Generic;

namespace Deposit.Domain.Entities
{
    public class ProviderOrderItem : Entity
    {
        public Guid ProviderOrderId { get; protected set; }
        public ProviderOrder ProviderOrder { get; protected set; }
        public Guid ProductId { get; protected set; }
        public Product Product { get; protected set; }
        public float Amount { get; protected set; }
        public float Price { get; protected set; }
        public float TotalValue { get; protected set; }
        public List<ProviderDeposit> Deposits { get; protected set; }

        private ProviderOrderItem() :
            base()
        { }

        public static ProviderOrderItem MakeProviderOrderItem(ProviderOrder providerOrder, Product product, float amount)
        {
            if (providerOrder == null)
                throw new ArgumentNullException("Provider order must have a value.");

            if (product == null)
                throw new ArgumentNullException("Product must have a value.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            var providerOrderItem = new ProviderOrderItem();

            providerOrderItem.ProviderOrderId = providerOrder.Id;
            providerOrderItem.ProviderOrder = providerOrder;
            providerOrderItem.ProductId = product.Id;
            providerOrderItem.Product = product;
            providerOrderItem.Amount = amount;
            providerOrderItem.Price = product.Price;
            providerOrderItem.TotalValue = providerOrderItem.Price * providerOrderItem.Amount;
            providerOrderItem.Deposits = new List<ProviderDeposit>();

            providerOrder.UpdateTotalValue(providerOrderItem.TotalValue);

            var deposit = ProviderDeposit.MakeProviderDeposit(providerOrderItem, amount, DepositMovement.In);
            providerOrderItem.Deposits.Add(deposit);

            return providerOrderItem;
        }

        public void ChangeAmount(float amount)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is deleted.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be larger than 0.");

            if (Amount == amount)
                throw new ArgumentException("Amount can't be the same as before.");

            var difference = amount - Amount;
            var movementType = DepositMovement.In;

            if (difference < 0)
            {
                difference = -difference;
                movementType = DepositMovement.Out;
            }

            Amount = amount;
            var oldValue = TotalValue;
            TotalValue = Price * Amount;

            var deposit = ProviderDeposit.MakeProviderDeposit(this, difference, movementType);
            Deposits.Add(deposit);

            difference = TotalValue - oldValue;
            ProviderOrder.UpdateTotalValue(difference);
        }

        public void ChangePrice(float price)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is deleted.");

            if (price <= 0)
                throw new ArgumentException("Price must be larger than 0.");

            Price = price;
            var oldValue = TotalValue;
            TotalValue = Price * Amount;

            var difference = TotalValue - oldValue;
            ProviderOrder.UpdateTotalValue(difference);
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order item is already deleted.");
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            var deletedMovementItem = ProviderDeposit.MakeProviderDeposit(this, Amount, DepositMovement.Out);
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            base.Delete();
        }
    }
}