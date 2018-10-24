using System;

namespace Deposit.Application.Dtos
{
    public class ProviderOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid ProviderId { get; set; }
        public ProviderOrderItemDto[] Items { get; set; }
    }
}