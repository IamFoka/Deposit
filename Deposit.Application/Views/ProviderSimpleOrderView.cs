using System;
using System.Collections.Generic;

namespace Deposit.Application.Views
{
    public class ProviderSimpleOrderView
    {
        public Guid Id { get; set; }
        public string RegisterDate { get; set; }
        public int RegisterNumber { get; set; }
        public float TotalValue { get; set; }
    }
}