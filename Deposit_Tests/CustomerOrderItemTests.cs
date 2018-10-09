using System;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class CustomerOrderItemTests
    {
        [Fact]
        public void MakeItem_NullOrder_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => CustomerOrderItem.MakeCustomerOrderItem(null, product, 5));
        }
        
        [Fact]
        public void MakeItem_NullProduct_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => CustomerOrderItem.MakeCustomerOrderItem(customerOrder, null, 5));
        }
        
        [Fact]
        public void MakeItem_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 0));
        }
        
        [Fact]
        public void MakeItem_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, -5));
        }
        
        [Fact]
        public void MakeItem_ValidParameters_ItemCreated()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // assert
            Assert.Equal(customerOrderItem.Amount, 5);
            Assert.Equal(customerOrderItem.Deposits.Count, 1);
            Assert.Equal(customerOrderItem.Price, 15);
            Assert.Equal(customerOrderItem.ProductId, product.Id);
            Assert.Equal(customerOrderItem.CustomerOrderId, customerOrder.Id);
            Assert.Equal(customerOrderItem.TotalValue, 75);
            Assert.Equal(customerOrder.TotalValue, 75);
            Assert.Equal(customer.TotalSpent, 75);
            Assert.Equal(product.Amount, 10);
        }
        
        [Fact]
        public void ChangeAmount_SameAmountAsBefore_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangeAmount(5));
        }
        
        [Fact]
        public void ChangeAmount_ZeroAmount_ExceptionThrown()
        {
            // arrange        
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangeAmount(0));
        }
        
        [Fact]
        public void ChangeAmount_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangeAmount(-10));
        }
        
        [Fact]
        public void ChangeAmount_AmountBiggerThanPreviousBiggerThanProducts_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangeAmount(20));
        }
        
        [Fact]
        public void ChangeAmount_AmountBiggerThanPreviousSmallerThanProducts_AmountUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            customerOrderItem.ChangeAmount(10);
            
            // assert
            Assert.Equal(customerOrderItem.Amount, 10);
            Assert.Equal(customerOrderItem.Deposits.Count, 2);
            Assert.Equal(customerOrderItem.Price, 15);
            Assert.Equal(customerOrderItem.TotalValue, 150);
            Assert.Equal(customerOrder.TotalValue, 150);
            Assert.Equal(customer.TotalSpent, 150);
            Assert.Equal(product.Amount, 5);
        }
        
        [Fact]
        public void ChangeAmount_AmountSmallerThanPrevious_AmountUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            customerOrderItem.ChangeAmount(3);
            
            // assert
            Assert.Equal(customerOrderItem.Amount, 3);
            Assert.Equal(customerOrderItem.Deposits.Count, 2);
            Assert.Equal(customerOrderItem.Price, 15);
            Assert.Equal(customerOrderItem.TotalValue, 45);
            Assert.Equal(customerOrder.TotalValue, 45);
            Assert.Equal(customer.TotalSpent, 45);
            Assert.Equal(product.Amount, 12);
        }
        
        [Fact]
        public void ChangePrice_SamePriceAsBefore_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangePrice(15));
        }
        
        [Fact]
        public void ChangePrice_ZeroPrice_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangePrice(0));
        }
        
        [Fact]
        public void ChangePrice_NegativePrice_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customerOrderItem.ChangePrice(-15));
        }
        
        [Fact]
        public void ChangePrice_PriceSmallerThanPrevious_PriceUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            customerOrderItem.ChangePrice(10);
            
            // assert
            Assert.Equal(customerOrderItem.Amount, 5);
            Assert.Equal(customerOrderItem.Deposits.Count, 1);
            Assert.Equal(customerOrderItem.Price, 10);
            Assert.Equal(customerOrderItem.TotalValue, 50);
            Assert.Equal(customerOrder.TotalValue, 50);
            Assert.Equal(customer.TotalSpent, 50);
            Assert.Equal(product.Amount, 10);
        }
        
        [Fact]
        public void ChangePrice_PriceBiggerThanPrevious_PriceUpdated()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);
            
            // act
            customerOrderItem.ChangePrice(20);
            
            // assert
            Assert.Equal(customerOrderItem.Amount, 5);
            Assert.Equal(customerOrderItem.Deposits.Count, 1);
            Assert.Equal(customerOrderItem.Price, 20);
            Assert.Equal(customerOrderItem.TotalValue, 100);
            Assert.Equal(customerOrder.TotalValue, 100);
            Assert.Equal(customer.TotalSpent, 100);
            Assert.Equal(product.Amount, 10);
        }

        [Fact]
        public void CustomerOrderItem_Delete()
        {
            // arrange
            var customer = Customer.MakeCustomer("julio", "05526485072", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(7, customer);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customerOrderItem = CustomerOrderItem.MakeCustomerOrderItem(customerOrder, product, 5);

            // act
            customerOrderItem.Delete();

            // assert
            Assert.True(customerOrderItem.IsDeleted);
        }
    }
}