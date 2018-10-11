using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;
using Deposit.Views;

namespace Deposit.Services
{
    public class ProviderOrderItemServices
    {
        public void UpdateProviderOrderItem(IRepository<ProviderOrderItem> repository, Guid id, ProviderOrderItemDto dto)
        {
            var item = repository.Read(id);
            
            if (item == null)
                throw new ArgumentException("Item not found.");
            
            if (dto.Amount != 0)
                item.ChangeAmount(dto.Amount);
            
            if (dto.Price != 0)
                item.ChangePrice(dto.Price);
        }
    }
}