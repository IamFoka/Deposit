using System;
using System.Collections.Generic;

namespace Deposit.Views
{
    public class ProviderWithOrdersView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public List<SimpleProviderOrderView> Orders { get; set; }

        public class SimpleProviderOrderView
        {
            public Guid Id { get; set; }
            public string RegisterDate { get; set; }
            public int RegisterNumber { get; set; }
            public float TotalValue { get; set; }
        }
    }
}