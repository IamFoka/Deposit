using System;
using System.Linq;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class CustomerDepositTest
    {
        [Fact]
        public void MakeCustomerDeposit_NullCustomerOrderItem_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentNullException>(() => CustomerDeposit.MakeCustomerDeposit(null, 5, DepositMovement.Out));
        }
        
        [Fact]
        public void MakeCustomerDeposit_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customer = Customer.MakeCustomer("Julio", "05908990078", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.AddItem(product, 5);

            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerDeposit.MakeCustomerDeposit(customerOrder.CustomerOrderItems.First(), 0, DepositMovement.Out));
        }
        
        [Fact]
        public void MakeCustomerDeposit_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customer = Customer.MakeCustomer("Julio", "05908990078", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.AddItem(product, 5);

            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerDeposit.MakeCustomerDeposit(customerOrder.CustomerOrderItems.First(), -5, DepositMovement.Out));
        }
        
        [Fact]
        public void MakeCustomerDeposit_AmountBiggerTanProducts_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customer = Customer.MakeCustomer("Julio", "05908990078", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.AddItem(product, 5);

            // act
            var e = Assert.Throws<ArgumentException>(() => CustomerDeposit.MakeCustomerDeposit(customerOrder.CustomerOrderItems.First(), 20, DepositMovement.Out));
        }
        
        [Fact]
        public void MakeCustomerDeposit_ValidParameters_DepositCreated()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customer = Customer.MakeCustomer("Julio", "05908990078", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.AddItem(product, 5);

            // act
            var customerDeposit = CustomerDeposit.MakeCustomerDeposit(customerOrder.CustomerOrderItems.First(), 5, DepositMovement.Out);
            
            // assert
            Assert.Equal(customerDeposit.Amount, 5);
            Assert.Equal(customerDeposit.Sku, product.Sku);
            
        }

        [Fact]
        public void CustomerDeposit_Delete()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var customer = Customer.MakeCustomer("Julio", "05908990078", DateTime.Now);
            var customerOrder = CustomerOrder.MakeCustomerOrder(5, customer);
            customerOrder.AddItem(product, 5);
            var customerDeposit = CustomerDeposit.MakeCustomerDeposit(customerOrder.CustomerOrderItems.First(), 5, DepositMovement.Out);

            // act 
            var e = Assert.Throws<InvalidOperationException>(() => customerDeposit.Delete());
        }
    }
}