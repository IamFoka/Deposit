using System;
using System.Linq;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class ProviderDepositTest
    {
        [Fact]
        public void MakeProviderDeposit_NullCustomerOrderItem_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentNullException>(() => ProviderDeposit.MakeProviderDeposit(null, 5, DepositMovement.In));
        }
        
        [Fact]
        public void MakeProviderDeposit_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            providerOrder.AddItem(product, 5);

            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderDeposit.MakeProviderDeposit(providerOrder.ProviderOrderItems.First(), 0, DepositMovement.In));
        }
        
        [Fact]
        public void MakeProviderDeposit_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            providerOrder.AddItem(product, 5);

            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderDeposit.MakeProviderDeposit(providerOrder.ProviderOrderItems.First(), -5, DepositMovement.In));
        }
        
        [Fact]
        public void MakeCustomerDeposit_ValidParameters_DepositCreated()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            providerOrder.AddItem(product, 5);

            // act
            var providerDeposit = ProviderDeposit.MakeProviderDeposit(providerOrder.ProviderOrderItems.First(), 5, DepositMovement.In);
            
            // assert
            Assert.Equal(providerDeposit.Amount, 5);
            Assert.Equal(providerDeposit.Sku, product.Sku);
            
        }        

        [Fact]
        public void DeleteProviderDeposit_ValidParameters_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            providerOrder.AddItem(product, 5);
            var providerDeposit = ProviderDeposit.MakeProviderDeposit(providerOrder.ProviderOrderItems.First(), 5, DepositMovement.In);

            //act
            var e = Assert.Throws<InvalidOperationException>(() => providerDeposit.Delete());
        }
    }
}