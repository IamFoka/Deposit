using System;

namespace Deposit.Controllers
{
    public class CustomerOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerOrderItemDto[] Items { get; set; }
    }
}