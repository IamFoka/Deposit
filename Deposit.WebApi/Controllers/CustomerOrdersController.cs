using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Application.Views;
using Deposit.Application.Dtos;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customer-orders")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly CustomerOrderServices _customerOrderServices;

        public CustomerOrdersController(CustomerOrderServices customerOrderServices)
        {
            _customerOrderServices = customerOrderServices;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var customerOrders = _customerOrderServices.GetAllOrders();

            if (customerOrders == null)
                return NotFound();

            return Ok(customerOrders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrder(Guid id)
        {
            var customerOrder = _customerOrderServices.GetOrder(id);

            if (customerOrder == null)
                return NotFound();

            return Ok(customerOrder);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomerOrder([FromBody] CustomerOrderDto dto)
        {
            try
            {
                return Ok(_customerOrderServices.CreateOrder(dto));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCostumerOrder(Guid id)
        {
            try
            {
                _customerOrderServices.DeleteCustomerOrder(id);
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
        public IActionResult AddCustomerOrderItem(Guid id, [FromBody] CustomerOrderItemDto dto)
        {
            try
            {
                CustomerOrderItemView  itemView = _customerOrderServices.AddItem(id, dto);
                return Ok(itemView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}