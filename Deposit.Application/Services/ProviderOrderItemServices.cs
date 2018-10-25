using System;
using System.Linq;
using Deposit.Data.Interfaces;
using Deposit.Domain.Entities;
using Deposit.Application.Views;
using Deposit.Application.Dtos;

namespace Deposit.Application.Services
{
    public class ProviderOrderItemServices
    {   
        private readonly IRepository<ProviderOrderItem> _repository;

        public ProviderOrderItemServices(IRepository<ProviderOrderItem> repository)
        {
            _repository = repository;
        }

        public void UpdateProviderOrderItem(Guid id, ProviderOrderItemDto dto)
        {
            var item = _repository.GetAll().FirstOrDefault(i => i.Id == id);
            
            if (item == null)
                throw new ArgumentException("Item not found.");
            
            if (dto.Amount != 0)
                item.ChangeAmount(dto.Amount);
            
            if (dto.Price != 0)
                item.ChangePrice(dto.Price);
        }   
    }
}