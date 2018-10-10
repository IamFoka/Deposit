using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Deposit.Services;
using Deposit.Models;

namespace Deposit.Controllers
{
    [Route("deposit/customers/")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Customer>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllCustomers()
        {
            var customerRepository = new CustomerRepository();
            var customerServices = new CustomerServices();
            var customers = customerServices.GetAllCustomers(customerRepository);

            if (customers.Count == 0)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomer(Guid id)
        {
            var customerRepository = new CustomerRepository();
            var customerServices = new CustomerServices();
            var customer = customerServices.GetCustomer(customerRepository, id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var customerRepository = new CustomerRepository();
            var customerServices = new CustomerServices();

            try
            {
                return Ok(customerServices.CreateCustomer(customerRepository, customerDto.Name, customerDto.Cpf, customerDto.BirthDate));
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
            var customerRepository = new CustomerRepository();
            var customerServices = new CustomerServices();
            
            try
            {
                customerServices.DeleteCustomer(customerRepository, id);
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
            var customerRepository = new CustomerRepository();
            var customerServices = new CustomerServices();

            try
            {
                customerServices.UpdateCustomer(customerRepository, id, dto.Name, dto.Cpf);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Customer not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders(Guid id)
        {
            var customerRepository = new CustomerRepository();
            var customerOrderRepository = new CustomerOrderRepository();

            var customerServices = new CustomerServices();

            var customer = customerServices.GetCustomer(customerRepository, id);

            if (customer == null)
                return NotFound();

            return NotFound(); // @TODO ver como serializar isso pra json
        }
    }
}