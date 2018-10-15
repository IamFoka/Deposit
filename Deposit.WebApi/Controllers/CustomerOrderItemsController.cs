using System;
using Deposit.WebApi.Dtos;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customer-orders/items")]
    public class CustomerOrderItemsController : ControllerBase
    {
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomerOrderItem(Guid id, [FromBody] CustomerOrderItemDto dto)
        {
            var repository = new CustomerOrderItemRepository();
            var services = new CustomerOrderItemServices();

            try
            {
                services.UpdateCustomerOrderItem(repository, id, dto);
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