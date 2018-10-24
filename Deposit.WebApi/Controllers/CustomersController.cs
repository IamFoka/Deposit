using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Application.Dtos;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.Application.Views;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customers/")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerServices _customerServices;
        private readonly CustomerOrderServices _customerOrderServices;

        public CustomersController(CustomerServices customerServices, CustomerOrderServices customerOrderServices)
        {
            _customerServices = customerServices;
            _customerOrderServices = customerOrderServices;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Customer>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerServices.GetAllCustomers();

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomer(Guid id)
        {
            CustomerView customer; 

            try
            {
                customer = _customerServices.GetCustomer(id);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                return Ok(_customerServices.CreateCustomer(customerDto.Name, customerDto.Cpf, customerDto.BirthDate));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(Guid id)
        {            
            try
            {
                _customerServices.DeleteCustomer(id);
                return Ok();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerDto dto)
        {
            try
            {
                _customerServices.UpdateCustomer(id, dto.Name, dto.Cpf);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Customer not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
            
        }
        
        [HttpGet("orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var customers = _customerServices.GetAllOrders();

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders(Guid id)
        {
            var orders = _customerOrderServices.GetAllOrders(id);

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet("top-customers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetTopCustomers()
        {
            var customers = _customerServices.GetTopCustomers();

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
    }
}