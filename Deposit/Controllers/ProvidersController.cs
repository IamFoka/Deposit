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
    [Route("deposit/providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Provider>))]
        [ProducesResponseType(404)]
        public IActionResult GetProviders()
        {
            var repository = new ProviderRepository();
            var services = new ProviderServices();
            var providers = services.GetAllProviders(repository);

            if (providers.Count == 0)
                return NotFound();

            return Ok(providers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Provider))]
        [ProducesResponseType(404)]
        public IActionResult GetProvider(Guid id)
        {
            var repository = new ProviderRepository();
            var services = new ProviderServices();
            var provider = services.GetProvider(repository, id);

            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Provider))]
        [ProducesResponseType(400)]
        public IActionResult CreateProvider([FromBody] ProviderDto dto)
        {
            var repository = new ProviderRepository();
            var services = new ProviderServices();

            try
            {
                return Ok(services.CreateProvider(repository, dto));
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
            var repository = new ProviderRepository();
            var services = new ProviderServices();
            
            try
            {
                services.DeleteProvider(repository, id);
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
            var repository = new ProviderRepository();
            var services = new ProviderServices();

            try
            {
                services.UpdateProvider(repository, id, dto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Provider not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/orders")]
        [ProducesResponseType(404)]
        public IActionResult GetProviderOrders(Guid id)
        {
            var repository = new ProviderRepository();
            var orderRepository = new ProviderOrderRepository();

            var services = new ProviderServices();

            var provider = services.GetProvider(repository, id);

            if (provider == null)
                return NotFound();

            return NotFound(); // @TODO ver como serializar isso pra json
        }
    }
}