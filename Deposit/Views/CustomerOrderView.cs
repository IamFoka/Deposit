using System;

namespace Deposit.Views
{
    public class CustomerOrderView
    {
        public Guid Id { get; set; }
        public string RegisterDate { get; set; }
        public int RegisterNumber { get; set; }
        public CustomerView Customer { get; set; }
        public float TotalValue { get; set; }
    }
}