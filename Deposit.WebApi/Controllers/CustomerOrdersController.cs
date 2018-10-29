using System;
using System.Collections.Generic;
using Deposit.Data.Interfaces;
using Deposit.Application.Views;
using Deposit.Application.Dtos;
using Deposit.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Deposit.Domain.Entities;

namespace Deposit.WebApi.Controllers
{
    [Route("deposit/customer-orders")]
    [ApiController]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly IRepository<CustomerOrder> _repository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerOrdersController(IRepository<CustomerOrder> repository, IRepository<Product> productRepository, IRepository<Customer> customerRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerOrderView>))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrders()
        {
            var customerOrderServices = new CustomerOrderServices();
            var customerOrders = customerOrderServices.GetAllOrders(_repository);

            if (customerOrders == null)
                return NotFound();

            return Ok(customerOrders);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(404)]
        public IActionResult GetCustomerOrder(Guid id)
        {
            var customerOrderServices = new CustomerOrderServices();
            var customerOrder = customerOrderServices.GetOrder(_repository, id);

            if (customerOrder == null)
                return NotFound();

            return Ok(customerOrder);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CustomerOrderCompleteView))]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomerOrder([FromBody] CustomerOrderDto dto)
        {
            var customerOrderServices = new CustomerOrderServices();

            try
            {
                return Ok(customerOrderServices.CreateOrder(_repository, _customerRepository, _productRepository, dto));
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
            var customerOrderServices = new CustomerOrderServices();

            try
            {
                customerOrderServices.DeleteCustomerOrder(_repository, id);
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
        public IActionResult AddCustomerOrderItem(Guid id, [FromBody] CustomerOrderItemDto dto)
        {
            var services = new CustomerOrderServices();

            try
            {
                CustomerOrderItemView  itemView = services.AddItem(_repository, _productRepository, id, dto);
                return Ok(itemView);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}