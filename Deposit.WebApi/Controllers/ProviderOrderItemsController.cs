using System;
using Deposit.Data.Interfaces;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/provider-orders/items")]
    public class ProviderOrderItemsController : ControllerBase
    {
        private readonly IRepository<ProviderOrderItem> _repository;

        public ProviderOrderItemsController(IRepository<ProviderOrderItem> repository)
        {
            _repository = repository;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProviderOrderItem(Guid id, [FromBody] ProviderOrderItemDto dto)
        {
            var services = new ProviderOrderItemServices();

            try
            {
                services.UpdateProviderOrderItem(_repository, id, dto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Item not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
        }
    }
}