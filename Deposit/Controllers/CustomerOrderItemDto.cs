using System;

namespace Deposit.Controllers
{
    public class CustomerOrderItemDto
    {
        public Guid ProductId { get; set; }
        public float Amount { get; set; }
    }
}