using System;
using System.Collections.Generic;

namespace Deposit.Application.Dtos
{
    public class CustomerOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid CustomerId { get; set; }
        public List<CustomerOrderItemDto> Items { get; set; }
    }
}