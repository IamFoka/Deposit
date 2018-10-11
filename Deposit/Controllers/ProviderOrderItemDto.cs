using System;

namespace Deposit.Controllers
{
    public class ProviderOrderItemDto
    {
        public Guid ProductId { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}