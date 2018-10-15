using System;
using System.Collections.Generic;

namespace Deposit.Views
{
    public class CustomerOrderCompleteView
    {
        public Guid Id { get; set; }
        public string RegisterDate { get; set; }
        public int RegisterNumber { get; set; }
        public CustomerView Customer { get; set; }
        public float TotalValue { get; set; }
        public List<CustomerOrderItemView> Items { get; set; }
    }
}