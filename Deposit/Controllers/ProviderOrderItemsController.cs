using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Deposit.Services;
using Deposit.Models;
using Deposit.Views;

namespace Deposit.Controllers
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