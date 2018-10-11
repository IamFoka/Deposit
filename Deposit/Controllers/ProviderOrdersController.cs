using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Deposit.Services;
using Deposit.Models;
using Deposit.Views;

namespace Deposit.Controllers
{
    [Route("deposit/provider-orders")]
    [ApiController]
    public class ProviderOrdersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProviderOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var repository = new ProviderOrderRepository();
            var services = new ProviderOrderServices();
            var orders = services.GetAllOrders(repository);

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProviderOrderView))]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrder(Guid id)
        {
            var repository = new ProviderOrderRepository();
            var services = new ProviderOrderServices();
            var order = services.GetOrder(repository, id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProviderOrderView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProviderOrder([FromBody] ProviderOrderDto dto)
        {
            var repository = new ProviderOrderRepository();
            var customerRepository = new ProviderRepository();
            var productRepository = new ProductRepository();
            var services = new ProviderOrderServices();

            try
            {
                return Ok(services.CreateOrder(repository, customerRepository, productRepository, dto));
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
            var repository = new ProviderOrderRepository();
            var services = new ProviderOrderServices();

            try
            {
                services.DeleteProviderOrder(repository, id);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}