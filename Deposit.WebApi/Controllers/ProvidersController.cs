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
        private readonly ProviderServices _providerServices;
        private readonly ProviderOrderServices _providerOrderServices;
        public ProvidersController(ProviderServices providerServices, ProviderOrderServices providerOrderServices)
        {
            _providerServices = providerServices;
            _providerOrderServices = providerOrderServices;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProviderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetProviders()
        {
            var providers = _providerServices.GetAllProviders();

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
            ProviderView provider;

            try
            {
                provider = _providerServices.GetProvider(id);
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
            try
            {
                return Ok(_providerServices.CreateProvider(dto));
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
            try
            {
                _providerServices.DeleteProvider(id);
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
            try
            {
                _providerServices.UpdateProvider(id, dto);
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
            var providers = _providerServices.GetAllOrders();

            if (providers.Count == 0)
                return NotFound();

            return Ok(providers);
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrders(Guid id)
        {

            var orders = _providerOrderServices.GetAllOrders(id);

            if (orders.Count == 0)
                return NotFound();

            return Ok(orders);
        }
    }
}