using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Views;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;
using Deposit.WebApi.Dtos;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/provider-orders")]
    [ApiController]
    public class ProviderOrdersController : ControllerBase
    {
        private readonly IRepository<ProviderOrder> _repository;
        private readonly IRepository<Provider> _providerRepository;
        private readonly IRepository<Product> _productRepository;

        public ProviderOrdersController(IRepository<ProviderOrder> repository, IRepository<Provider> providerRepository, IRepository<Product> productRepository)
        {
            _repository = repository;
            _providerRepository = providerRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProviderOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var services = new ProviderOrderServices();
            var orders = services.GetAllOrders(_repository);
            
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
            var services = new ProviderOrderServices();
            var order = services.GetOrder(_repository, id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProviderOrderView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProviderOrder([FromBody] ProviderOrderDto dto)
        {
            var services = new ProviderOrderServices();

            try
            {
                return Ok(services.CreateOrder(_repository, _providerRepository, _productRepository, dto));
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
            var services = new ProviderOrderServices();

            try
            {
                services.DeleteProviderOrder(_repository, id);
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
            var services = new ProviderOrderServices();

            try
            {
                ProviderOrderItemView  itemView = services.AddItem(_repository, _productRepository, id, dto);
                return Ok(itemView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}