using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Deposit.Application.Dtos
{
    public class ProviderOrderDto
    {
        public int RegisterNumber { get; set; }
        public Guid ProviderId { get; set; }
        public List<ProviderOrderItemDto> Items { get; set; }
    }
}