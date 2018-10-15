using System;
using System.Collections.Generic;
using Deposit.Views;
using Deposit.WebApi.Dtos;
using Deposit.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductView>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllProducts()
        {
            var productRepository = new ProductRepository();
            var productServices = new ProductServices();
            var products = productServices.GetAllProducts(productRepository);

            if (products.Count == 0)
                return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductView))]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(Guid id)
        {
            var productRepository = new ProductRepository();
            var productServices = new ProductServices();
            var product = productServices.GetProduct(productRepository, id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            var productRepository = new ProductRepository();
            var productServices = new ProductServices();

            try
            {
                return Ok(productServices.CreateProduct(productRepository, productDto));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(Guid id)
        {
            var productRepository = new ProductRepository();
            var productServices = new ProductServices();

            try
            {
                productServices.DeleteProduct(productRepository, id);
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
        public IActionResult UpdateCustomer(Guid id, [FromBody] ProductDto productDto)
        {
            var productRepository = new ProductRepository();
            var productServices = new ProductServices();

            try
            {
                productServices.UpdateProduct(productRepository, id, productDto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                if (e.Message == "Customer not found.")
                    return NotFound();

                return BadRequest(e.Message);
            }
        }
    }
}