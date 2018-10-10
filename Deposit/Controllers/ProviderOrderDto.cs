using System;

namespace Deposit.Controllers
{
    public class ProviderOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid ProviderId { get; set; }
    }
}