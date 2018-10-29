using System;

namespace Deposit.Application.Views
{
    public class ProductView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }
    }
}