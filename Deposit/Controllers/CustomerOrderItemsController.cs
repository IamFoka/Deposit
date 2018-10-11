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