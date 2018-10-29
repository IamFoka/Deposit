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
        private readonly ProductServices _productServices;

        public ProductsController(ProductServices productServices)
        {
            _productServices = productServices;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductView>))]
        [ProducesResponseType(404)]
        public IActionResult GetAllProducts()
        {
            var products = _productServices.GetAllProducts();

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
            ProductView product;

            try
            {
                product = _productServices.GetProduct(id);
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
            try
            {
                return Ok(_productServices.CreateProduct(productDto));
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
            try
            {
                _productServices.DeleteProduct(id);
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
            try
            {
                _productServices.UpdateProduct(id, productDto);
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