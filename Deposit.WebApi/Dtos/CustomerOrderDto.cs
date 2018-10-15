using System;

namespace Deposit.WebApi.Dtos
{
    public class CustomerOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerOrderItemDto[] Items { get; set; }
    }
}