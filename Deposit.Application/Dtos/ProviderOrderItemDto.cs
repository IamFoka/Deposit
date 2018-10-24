using System;

namespace Deposit.Application.Dtos
{
    public class ProviderOrderItemDto
    {
        public Guid ProductId { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}