﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Models
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
            return Providers.FirstOrDefault(c => c.Id == guid);
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
            return new List<Provider>(Providers);
        }
    }
}