using System;
using System.Collections.Generic;

namespace Deposit.Application.Views
{
    public class CustomerWithOrdersView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public IEnumerable<CustomerSimpleOrderView> Orders { get; set; }
    }
}