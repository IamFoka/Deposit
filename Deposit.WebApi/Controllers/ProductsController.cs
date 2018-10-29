using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Application.Views;
using Deposit.Application.Dtos;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Data.Repositories;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductView>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllProducts()
        {
            var productServices = new ProductServices();
            var products = productServices.GetAllProducts(_repository);

            if (products.Count == 0)
                return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductView))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(Guid id)
        {
            var productServices = new ProductServices();
            ProductView product;

            try
            {
                product = productServices.GetProduct(_repository, id);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
                

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductView))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            var productServices = new ProductServices();

            try
            {
                return Ok(productServices.CreateProduct(_repository, productDto));
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
            var productServices = new ProductServices();

            try
            {
                productServices.DeleteProduct(_repository, id);
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
            var productServices = new ProductServices();

            try
            {
                productServices.UpdateProduct(_repository, id, productDto);
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