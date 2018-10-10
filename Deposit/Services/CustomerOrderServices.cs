using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Controllers;
using Deposit.Models;

namespace Deposit.Services
{
    public class CustomerOrderServices
    {
        public List<CustomerOrder> GetAllOrders(IRepository<CustomerOrder> repository)
        {
            return repository.ReadAll();
        }

        public CustomerOrder GetOrder(IRepository<CustomerOrder> repository, Guid id)
        {
            return repository.Read(id);
        }

        public CustomerOrder CreateOrder(IRepository<CustomerOrder> repository, 
            IRepository<Customer> customerRepository, CustomerOrderDto customerOrderDto)
        {
            var customer = customerRepository.Read(customerOrderDto.CustomerId);
            
            if (customer == null)
                throw new ArgumentException("Customer not found.");

            var customerOrder = CustomerOrder.MakeCustomerOrder(customerOrderDto.RegisterNumber, customer);
            
            repository.Add(customerOrder);
            return customerOrder;
        }

        public void DeleteCustomerOrder(IRepository<CustomerOrder> repository, Guid id)
        {
            repository.Delete(id);
        }
    }
}