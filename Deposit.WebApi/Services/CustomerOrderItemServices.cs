using System;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Services
{
    public class CustomerOrderItemServices
    {
        public void UpdateCustomerOrderItem(IRepository<CustomerOrderItem> repository, Guid id, CustomerOrderItemDto dto)
        {
            var item = repository.ListAll().FirstOrDefault(i => i.Id == id);
            
            if (item == null)
                throw new ArgumentException("Item not found.");
            
            if (dto.Amount != 0)
                item.ChangeAmount(dto.Amount);
            
            if (dto.Price != 0)
                item.ChangePrice(dto.Price);
        }
    }
}