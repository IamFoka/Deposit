using System;
using System.Collections.Generic;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;

namespace Deposit.Data.Repositories
{
    public class ProviderRepository : IRepository<Provider>
    {
        private static List<Provider> Providers { get; set; }

        public ProviderRepository()
        {
            if (Providers == null)
                Providers = new List<Provider>();
        }

        public void Add(Provider provider)
        {
            Providers.Add(provider);
        }

        public Provider Read(Guid guid)
        {
            return Providers.FirstOrDefault(c => c.Id == guid && !c.IsDeleted);
        }

        public void Update(Guid guid, Provider t)
        {

        }

        public void Delete(Guid guid)
        {
            var provider = Read(guid);
            provider.Delete();
        }

        public List<Provider> ReadAll()
        {
            var l = new List<Provider>();
            
            foreach (var i in Providers)
                if (!i.IsDeleted)
                    l.Add(i);

            return l;
        }
    }
}
