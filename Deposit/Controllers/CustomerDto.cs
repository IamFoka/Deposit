using System;

namespace Deposit.Controllers
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
    }
}