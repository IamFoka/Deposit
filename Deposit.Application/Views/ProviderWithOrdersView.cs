using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Deposit.Application.Views
{
    public class ProviderWithOrdersView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public IEnumerable<ProviderSimpleOrderView> Orders { get; set; }
    }
}