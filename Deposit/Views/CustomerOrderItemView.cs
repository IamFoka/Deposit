using System;

namespace Deposit.Views
{
    public class CustomerOrderItemView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Product { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
        public float TotalValue { get; set; }
    }
}