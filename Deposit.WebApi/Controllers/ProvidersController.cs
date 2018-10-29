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
    [Route("deposit/providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IRepository<Provider> _repository;
        private readonly IRepository<ProviderOrder> _orderRepository;

        public ProvidersController(IRepository<Provider> repository, IRepository<ProviderOrder> orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProviderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetProviders()
        {
            var services = new ProviderServices();
            var providers = services.GetAllProviders(_repository);

            if (providers.Count == 0)
                return NotFound();

            return Ok(providers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProviderView))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetProvider(Guid id)
        {
            var services = new ProviderServices();
            ProviderView provider;

            try
            {
                provider = services.GetProvider(_repository, id);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProviderView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProvider([FromBody] ProviderDto dto)
        {
            var services = new ProviderServices();

            try
            {
                return Ok(services.CreateProvider(_repository, dto));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProvider(Guid id)
        {
            var services = new ProviderServices();
            
            try
            {
                services.DeleteProvider(_repository, id);
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
        public IActionResult UpdateProvider(Guid id, [FromBody] ProviderDto dto)
        {
            var services = new ProviderServices();

            try
            {
                services.UpdateProvider(_repository, id, dto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Provider not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
        }

        [HttpGet("orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrders()
        {
            var services = new ProviderServices();

            var providers = services.GetAllOrders(_repository, _orderRepository);

            if (providers.Count == 0)
                return NotFound();

            return Ok(providers);
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrders(Guid id)
        {
            var services = new ProviderOrderServices();

            var orders = services.GetAllOrders(_orderRepository, id);

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }
    }
}