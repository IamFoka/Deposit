using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.WebApi.Dtos;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customers/")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<CustomerOrder> _orderRepository;

        public CustomersController(IRepository<Customer> repository, IRepository<CustomerOrder> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Customer>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllCustomers()
        {
            var customerServices = new CustomerServices();
            var customers = customerServices.GetAllCustomers(_repository);

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomer(Guid id)
        {
            var customerServices = new CustomerServices();
            var customer = customerServices.GetCustomer(_repository, id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var customerServices = new CustomerServices();

            try
            {
                return Ok(customerServices.CreateCustomer(_repository, customerDto.Name, customerDto.Cpf, customerDto.BirthDate));
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
            var customerServices = new CustomerServices();
            
            try
            {
                customerServices.DeleteCustomer(_repository, id);
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
            var customerServices = new CustomerServices();

            try
            {
                customerServices.UpdateCustomer(_repository, id, dto.Name, dto.Cpf);
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
            var services = new CustomerServices();

            var customers = services.GetAllOrders(_repository, _orderRepository);

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders(Guid id)
        {
            var services = new CustomerOrderServices();

            var orders = services.GetAllOrders(_orderRepository, id);

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }

        [HttpGet("top-customers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetTopCustomers()
        {
            var services = new CustomerServices();

            var customers = services.GetTopCustomers(_repository);

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }
    }
}