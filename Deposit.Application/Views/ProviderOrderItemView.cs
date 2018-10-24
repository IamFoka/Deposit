using System;

namespace Deposit.Application.Views
{
    public class ProviderOrderItemView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Product { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
        public float TotalValue { get; set; }
    }
}