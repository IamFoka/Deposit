using System;

namespace Deposit.WebApi.Dtos
{
    public class CustomerOrderItemDto
    {
        public Guid ProductId { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}