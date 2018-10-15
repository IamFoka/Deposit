using System;
using System.Collections.Generic;

namespace Deposit.Domain.Entities
{
    public class ProviderOrder : Entity
    {
        public DateTime RegisterDate { get; private set; }
        public int RegisterNumber { get; private set; }
        public Guid ProviderId { get; private set; }
        public Provider Provider { get; private set; }
        public float TotalValue { get; private set; }
        public List<ProviderOrderItem> ProviderOrderItems { get; private set; }

        protected ProviderOrder() :
            base()
        { }

        public static ProviderOrder MakeProviderOrder(int registerNumber, Provider provider)
        {
            if (registerNumber <= 0)
                throw new ArgumentException("Register number must be larger than 0.");

            if (provider == null)
                throw new ArgumentNullException("Provider must have a value.");

            var providerOrder = new ProviderOrder();

            providerOrder.RegisterDate = DateTime.Now;
            providerOrder.RegisterNumber = registerNumber;
            providerOrder.ProviderId = provider.Id;
            providerOrder.Provider = provider;
            providerOrder.TotalValue = 0;
            providerOrder.ProviderOrderItems = new List<ProviderOrderItem>();

            return providerOrder;
        }

        public void AddItem(Product product, float amount)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order is deleted.");

            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(this, product, amount);
            ProviderOrderItems.Add(providerOrderItem);
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
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Order is already deleted.");

            foreach (var item in ProviderOrderItems)
                item.Delete();

            base.Delete();
        }
    }

}