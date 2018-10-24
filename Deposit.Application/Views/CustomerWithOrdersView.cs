using System;
using System.Collections.Generic;

namespace Deposit.Application.Views
{
    public class CustomerWithOrdersView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public List<SimpleCustomerOrderView> Orders { get; set; }

        public class SimpleCustomerOrderView
        {
            public Guid Id { get; set; }
            public string RegisterDate { get; set; }
            public int RegisterNumber { get; set; }
            public float TotalValue { get; set; }
        }
    }
}