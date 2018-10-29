using System;
using Deposit.Data.Interfaces;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/provider-orders/items")]
    public class ProviderOrderItemsController : ControllerBase
    {
        private readonly ProviderOrderItemServices _providerOrderItemServices;

        public ProviderOrderItemsController(ProviderOrderItemServices providerOrderItemServices)
        {
            _providerOrderItemServices = providerOrderItemServices;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProviderOrderItem(Guid id, [FromBody] ProviderOrderItemDto dto)
        {
            try
            {
                _providerOrderItemServices.UpdateProviderOrderItem(id, dto);
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