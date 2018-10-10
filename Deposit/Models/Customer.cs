using System;

namespace Deposit.Models
{
    public class Customer : Entity
    {
        public string Name { get; protected set; }
        public string Cpf { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public DateTime RegisterDate { get; protected set; }
        public float TotalSpent { get; protected set; }

        protected Customer() :
            base()
        { }

        public static Customer MakeCustomer(string name, string cpf, DateTime birthDate)
        {
            if (name == String.Empty)
            {
                throw new ArgumentException("Name can't be empty.");
            }

            if (cpf == string.Empty)
            {
                throw new ArgumentException("Cpf can't be empty.");
            }

            var customer = new Customer();

            customer.TotalSpent = 0;
            customer.RegisterDate = DateTime.Now;
            customer.BirthDate = birthDate;
            customer.UpdateDocumentation(name, cpf);

            return customer;

        }

        public void UpdateDocumentation(string name, string cpf)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Customer is deleted.");

            if (name != string.Empty)
                Name = name;

            if (cpf != string.Empty)
            {
                if (!BrValidator.ValidateCPF(cpf))
                    throw new ArgumentException("Invalid Cpf.");

                Cpf = cpf;
            }
        }

        public void UpdateTotalSpent(float value)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Customer is deleted.");

            if (value == 0)
            {
                throw new ArgumentException("Value can't be equal to zero 0.");
            }

            if (TotalSpent + value < 0)
                throw new ArgumentException("Total spent can't be lower than 0.");

            TotalSpent += value;
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Customer is already deleted.");

            base.Delete();
        }

    }
}