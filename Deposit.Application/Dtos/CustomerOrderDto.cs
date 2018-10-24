using System;

namespace Deposit.Application.Dtos
{
    public class CustomerOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerOrderItemDto[] Items { get; set; }
    }
}