using System;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/provider-orders/items")]
    public class ProviderOrderItemsController : ControllerBase
    {
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProviderOrderItem(Guid id, [FromBody] ProviderOrderItemDto dto)
        {
            var repository = new ProviderOrderItemRepository();
            var services = new ProviderOrderItemServices();

            try
            {
                services.UpdateProviderOrderItem(repository, id, dto);
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