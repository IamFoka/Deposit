using System;
using System.Collections.Generic;

namespace Deposit.Views
{
    public class ProviderOrderView
    {
        public Guid Id { get; set; }
        public string RegisterDate { get; set; }
        public int RegisterNumber { get; set; }
        public ProviderView Provider { get; set; }
        public float TotalValue { get; set; }
    }
}