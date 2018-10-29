using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Application.Views;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.Application.Dtos;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/provider-orders")]
    [ApiController]
    public class ProviderOrdersController : ControllerBase
    {
        private readonly ProviderOrderServices _providerOrderServices;
        public ProviderOrdersController(ProviderOrderServices providerOrderServices)
        {
            _providerOrderServices = providerOrderServices;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProviderOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var orders = _providerOrderServices.GetAllOrders();
            
            if (orders == null)
                return NotFound();

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProviderOrderView))]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrder(Guid id)
        {
            var order = _providerOrderServices.GetOrder(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProviderOrderView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProviderOrder([FromBody] ProviderOrderDto dto)
        {
            try
            {
                return Ok(_providerOrderServices.CreateOrder(dto));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProviderOrder(Guid id)
        {
            try
            {
                _providerOrderServices.DeleteProviderOrder(id);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddProviderOrderItem(Guid id, [FromBody] ProviderOrderItemDto dto)
        {
            try
            {
                ProviderOrderItemView  itemView = _providerOrderServices.AddItem(id, dto);
                return Ok(itemView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}