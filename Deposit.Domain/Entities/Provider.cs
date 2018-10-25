using System;
using System.Collections.ObjectModel;

namespace Deposit.Domain.Entities
{
    public class Provider : Entity
    {
        public string Name { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Cnpj { get; private set; }
        public Collection<ProviderOrder> ProviderOrders {get; private set;}

        protected Provider() :
            base()
        { }

        public static Provider MakeProvider(string name, string cnpj)
        {
            if (name == String.Empty)
            {
                throw new ArgumentException("Name can't be empty.");
            }

            if (cnpj == String.Empty)
            {
                throw new ArgumentException("Cnpj can't be empty.");
            }

            var provider = new Provider();

            provider.RegisterDate = DateTime.Now;
            provider.UpdateDocumentation(name, cnpj);

            return provider;

        }

        public void UpdateDocumentation(string name, string cnpj)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Provider is deleted.");

            if (!String.IsNullOrEmpty(name))
                Name = name;

            if (!String.IsNullOrEmpty(cnpj))
            {
                if (!BrValidator.ValidateCNPJ(cnpj))
                    throw new ArgumentException("Invalid Cnpj.");

                Cnpj = cnpj;
            }
        }

        public override void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Provider is already deleted.");

            base.Delete();
        }

    }
}