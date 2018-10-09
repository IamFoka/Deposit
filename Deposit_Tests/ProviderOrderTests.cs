using System;
using System.Linq;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class ProviderOrderTests
    {        
        [Fact]
        public void MakeProviderOrder_ZeroRegisterNumber_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            
            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderOrder.MakeProviderOrder(0, provider));
        }
        
        [Fact]
        public void MakeProviderOrder_NegativeRegisterNumber_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            
            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderOrder.MakeProviderOrder(-5, provider));
        }
        
        [Fact]
        public void MakeProviderOrder_NullProvider_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentNullException>(() => ProviderOrder.MakeProviderOrder(5, null));
        }
        
        [Fact]
        public void MakeProviderOrder_ValidParameters_OrderCreated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            
            // act
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            
            // assert
            Assert.Equal(providerOrder.ProviderId, provider.Id);
            Assert.Equal(providerOrder.RegisterNumber, 5);
            Assert.Equal(providerOrder.TotalValue, 0);
        }
        
        [Fact]
        public void UpdateTotalValue_ZeroValue_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrder.UpdateTotalValue(0));
        }
        
        [Fact]
        public void UpdateTotalValue_PositiveValue_TotalUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            
            // act
            providerOrder.UpdateTotalValue(50);
            providerOrder.UpdateTotalValue(25);
            
            // assert
            Assert.Equal(providerOrder.TotalValue, 75);
        }
        
        [Fact]
        public void UpdateTotalValue_NegativeValue_TotalUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            
            // act
            providerOrder.UpdateTotalValue(50);
            providerOrder.UpdateTotalValue(-25);
            
            // assert
            Assert.Equal(providerOrder.TotalValue, 25);
        }
        
        [Fact]
        public void UpdateTotalValue_NegativeTotal_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            providerOrder.UpdateTotalValue(25);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrder.UpdateTotalValue(-50));
        }

        [Fact]
        public void AddItem_NullProduct_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => providerOrder.AddItem(null, 5));
        }

        [Fact]
        public void AddItem_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrder.AddItem(product, 0));
        }

        [Fact]
        public void AddItem_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrder.AddItem(product, -5));
        }

        [Fact]
        public void AddItem_ValidParameters_ItemCreated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);
            var product = Product.MakeProduct("Test", "Test", 5, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(10);
            
            // act
            providerOrder.AddItem(product, 5);
            
            // assert
            var providerOrderItem = providerOrder.ProviderOrderItems.First();
            Assert.Equal(providerOrderItem.Amount, 5);
            Assert.Equal(providerOrderItem.ProductId, product.Id);
        }

        [Fact]
        public void ProviderOrder_Delete()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(5, provider);

            // act
            providerOrder.Delete();

            // assert
            Assert.True(providerOrder.IsDeleted);
        }
    }
}