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
    [Route("deposit/customers/orders")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerOrder>))]
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
        [ProducesResponseType(200, Type = typeof(CustomerOrder))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrder(Guid id)
        {
            var customerOrderRepository = new CustomerOrderRepository();
            var customerOrderServices = new CustomerOrderServices();
            var customerOrder = customerOrderServices.GetOrder(customerOrderRepository, id);

            if (customerOrder == null)
                return NotFound();

            return Ok(customerOrder); // @TODO retornar os itens junto da ordem
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CustomerOrder))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomerOrder([FromBody] CustomerOrderDto customerOrderDto)
        {
            var customerOrderRepository = new CustomerOrderRepository();
            var customerRepository = new CustomerRepository();
            var customerOrderServices = new CustomerOrderServices();

            try
            {
                return Ok(customerOrderServices.CreateOrder(customerOrderRepository, customerRepository,
                    customerOrderDto));
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