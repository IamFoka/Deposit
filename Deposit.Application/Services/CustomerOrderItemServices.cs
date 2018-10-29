using System;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class CustomerOrderItemServices
    {
        private readonly IRepository<CustomerOrderItem> _repository;

        public CustomerOrderItemServices(IRepository<CustomerOrderItem> repository)
        {
            _repository = repository;
        }

        public void UpdateCustomerOrderItem(Guid id, CustomerOrderItemDto dto)
        {
            var item = _repository.ListAll().FirstOrDefault(i => i.Id == id);
            
            if (item == null)
                throw new ArgumentException("Item not found.");
            
            if (dto.Amount != 0)
                item.ChangeAmount(dto.Amount);
            
            if (dto.Price != 0)
                item.ChangePrice(dto.Price);
        }
    }
}