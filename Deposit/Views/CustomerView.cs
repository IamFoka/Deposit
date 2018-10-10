using System;

namespace Deposit.Views
{
    public class CustomerView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string BirthDate { get; set; }
        public float TotalSpent { get; set; }
    }
}