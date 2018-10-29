using System;

namespace Deposit.Application.Dtos
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
    }
}