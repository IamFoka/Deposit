using System;
using System.Linq;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class CustomerOrderTests
    {
        [Fact]
        public void MakeCustomerOrder_ZeroRegisterNumber_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerOrder.MakeCustomerOrder(0, customer));
        }
        
        [Fact]
        public void MakeCustomerOrder_NegativeRegisterNumber_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerOrder.MakeCustomerOrder(-5, customer));
        }
        
        [Fact]
        public void MakeCustomerOrder_NullCustomer_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentNullException>(() => CustomerOrder.MakeCustomerOrder(5, null));
        }
        
        [Fact]
        public void MakeCustomerOrder_ValidParameters_OrderCreated()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            
            // act
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            
            // assert
            Assert.Equal(customerOrder.CustomerId, customer.Id);
            Assert.Equal(customerOrder.RegisterNumber, 5);
            Assert.Equal(customerOrder.TotalValue, 0);
        }
        
        [Fact]
        public void UpdateTotalValue_ZeroValue_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrder.UpdateTotalValue(0));
        }
        
        [Fact]
        public void UpdateTotalValue_PositiveValue_TotalUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            
            // act
            customerOrder.UpdateTotalValue(50);
            customerOrder.UpdateTotalValue(25);
            
            // assert
            Assert.Equal(customerOrder.TotalValue, 75);
        }
        
        [Fact]
        public void UpdateTotalValue_NegativeValue_TotalUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            
            // act
            customerOrder.UpdateTotalValue(50);
            customerOrder.UpdateTotalValue(-25);
            
            // assert
            Assert.Equal(customerOrder.TotalValue, 25);
        }
        
        [Fact]
        public void UpdateTotalValue_NegativeTotal_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.UpdateTotalValue(25);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrder.UpdateTotalValue(-50));
        }

        [Fact]
        public void AddItem_NullProduct_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => customerOrder.AddItem(null, 5));
        }

        [Fact]
        public void AddItem_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrder.AddItem(product, 0));
        }

        [Fact]
        public void AddItem_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrder.AddItem(product, -5));
        }

        [Fact]
        public void AddItem_AmountBiggerThanProducts_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrder.AddItem(product, 15));
        }

        [Fact]
        public void AddItem_ValidParameters_ItemCreated()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            customerOrder.AddItem(product, 5);
            
            // assert
            var customerOrderItem = customerOrder.CustomerOrderItems.First();
            Assert.Equal(customerOrderItem.Amount, 5);
            Assert.Equal(customerOrderItem.ProductId, product.Id);
        }

        [Fact]
        public void CustomerOrder_Delete()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);

            // act
            customerOrder.Delete();

            // assert
            Assert.True(customerOrder.IsDeleted);

        }

        [Fact]
        public void AddItem_CustomerOrder_Deleted()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customerOrder.Delete());
        }

        [Fact]
        public void UpdateTotalValue_CustomerOrder_Deleted()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customerOrder.UpdateTotalValue(1));
        }

        [Fact]
        public void DeleteCustomerOrder_DeletedCustomerOrder_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "18406811013", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customerOrder.Delete());
        }
    }
}