using System;
using System.Collections.Generic;
using Deposit.Views;
using Deposit.WebApi.Dtos;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customer-orders")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var customerOrderRepository = new CustomerOrderRepository();
            var customerOrderServices = new CustomerOrderServices();
            var customerOrders = customerOrderServices.GetAllOrders(customerOrderRepository);

            if (customerOrders == null)
                return NotFound();

            return Ok(customerOrders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrder(Guid id)
        {
            var customerOrderRepository = new CustomerOrderRepository();
            var customerOrderServices = new CustomerOrderServices();
            var customerOrder = customerOrderServices.GetOrder(customerOrderRepository, id);

            if (customerOrder == null)
                return NotFound();

            return Ok(customerOrder);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomerOrder([FromBody] CustomerOrderDto dto)
        {
            var customerOrderRepository = new CustomerOrderRepository();
            var customerRepository = new CustomerRepository();
            var productRepository = new ProductRepository();
            var customerOrderServices = new CustomerOrderServices();

            try
            {
                return Ok(customerOrderServices.CreateOrder(customerOrderRepository, customerRepository, productRepository, dto));
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
            var customerOrderRepository = new CustomerOrderRepository();
            var customerOrderServices = new CustomerOrderServices();

            try
            {
                customerOrderServices.DeleteCustomerOrder(customerOrderRepository, id);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}