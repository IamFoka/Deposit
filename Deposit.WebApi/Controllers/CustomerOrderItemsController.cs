using System;
using Deposit.Data.Interfaces;
using Deposit.Application.Dtos;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customer-orders/items")]
    public class CustomerOrderItemsController : ControllerBase
    {
        private readonly IRepository<CustomerOrderItem> _repository;

        public CustomerOrderItemsController(IRepository<CustomerOrderItem> repository)
        {
            _repository = repository;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomerOrderItem(Guid id, [FromBody] CustomerOrderItemDto dto)
        {
            var services = new CustomerOrderItemServices();

            try
            {
                services.UpdateCustomerOrderItem(_repository, id, dto);
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