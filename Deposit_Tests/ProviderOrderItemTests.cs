using System;
using System.Linq;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class ProviderOrderItemTests
    {
        [Fact]
        public void MakeItem_NullOrder_ExceptionThrown()
        {
            // arrange
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => ProviderOrderItem.MakeProviderOrderItem(null, product, 5));
        }
        
        [Fact]
        public void MakeItem_NullProduct_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            
            // act
            var e = Assert.Throws<ArgumentNullException>(() => ProviderOrderItem.MakeProviderOrderItem(providerOrder, null, 5));
        }
        
        [Fact]
        public void MakeItem_ZeroAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 0));
        }
        
        [Fact]
        public void MakeItem_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, -5));
        }
        
        [Fact]
        public void MakeItem_ValidParameters_ItemCreated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            
            // act
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // assert
            Assert.Equal(providerOrderItem.Amount, 5);
            Assert.Equal(providerOrderItem.Deposits.Count, 1);
            Assert.Equal(providerOrderItem.Price, 15);
            Assert.Equal(providerOrderItem.ProductId, product.Id);
            Assert.Equal(providerOrderItem.ProviderOrderId, providerOrder.Id);
            Assert.Equal(providerOrderItem.TotalValue, 75);
            Assert.Equal(providerOrder.TotalValue, 75);
            Assert.Equal(product.Amount, 5);
        }
        
        [Fact]
        public void ChangeAmount_SameAmountAsBefore_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangeAmount(5));
        }
        
        [Fact]
        public void ChangeAmount_ZeroAmount_ExceptionThrown()
        {
            // arrange        
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangeAmount(0));
        }
        
        [Fact]
        public void ChangeAmount_NegativeAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(15);
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangeAmount(-10));
        }
        
        [Fact]
        public void ChangeAmount_AmountSmallerThanProductAmount_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 10);
            product.UpdateAmount(-5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangeAmount(4));
        }
        
        [Fact]
        public void ChangeAmount_AmountSmallerThanPreviousBiggerThanProducts_AmountUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            providerOrderItem.ChangeAmount(3);
            
            // assert
            Assert.Equal(providerOrderItem.Amount, 3);
            Assert.Equal(providerOrderItem.Deposits.Count, 2);
            Assert.Equal(providerOrderItem.Price, 15);
            Assert.Equal(providerOrderItem.TotalValue, 45);
            Assert.Equal(providerOrder.TotalValue, 45);
            Assert.Equal(product.Amount, 3);
        }
        
        [Fact]
        public void ChangeAmount_AmountBiggerThanPrevious_AmountUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            providerOrderItem.ChangeAmount(10);
            
            // assert
            Assert.Equal(providerOrderItem.Amount, 10);
            Assert.Equal(providerOrderItem.Deposits.Count, 2);
            Assert.Equal(providerOrderItem.Price, 15);
            Assert.Equal(providerOrderItem.TotalValue, 150);
            Assert.Equal(providerOrder.TotalValue, 150);
            Assert.Equal(product.Amount, 10);
        }
        
        [Fact]
        public void ChangePrice_SamePriceAsBefore_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangePrice(15));
        }
        
        [Fact]
        public void ChangePrice_ZeroPrice_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangePrice(0));
        }
        
        [Fact]
        public void ChangePrice_NegativePrice_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => providerOrderItem.ChangePrice(-15));
        }
        
        [Fact]
        public void ChangePrice_PriceSmallerThanPrevious_PriceUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            providerOrderItem.ChangePrice(10);
            
            // assert
            Assert.Equal(providerOrderItem.Amount, 5);
            Assert.Equal(providerOrderItem.Deposits.Count, 1);
            Assert.Equal(providerOrderItem.Price, 10);
            Assert.Equal(providerOrderItem.TotalValue, 50);
            Assert.Equal(providerOrder.TotalValue, 50);
            Assert.Equal(product.Amount, 5);
        }
        
        [Fact]
        public void ChangePrice_PriceBiggerThanPrevious_PriceUpdated()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            
            // act
            providerOrderItem.ChangePrice(20);
            
            // assert
            Assert.Equal(providerOrderItem.Amount, 5);
            Assert.Equal(providerOrderItem.Deposits.Count, 1);
            Assert.Equal(providerOrderItem.Price, 20);
            Assert.Equal(providerOrderItem.TotalValue, 100);
            Assert.Equal(providerOrder.TotalValue, 100);
            Assert.Equal(product.Amount, 5);
        }  
        
        [Fact]
        public void DeleteProviderOrderItem_ValidParameters_ProviderOrderItemDeleted()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);

            //act
            providerOrderItem.Delete();

            // assert
            Assert.True(providerOrderItem.IsDeleted);
        }

        [Fact]
        public void DeleteProviderOrder_DeletedProviderOrder_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "09883060000174");
            var providerOrder = ProviderOrder.MakeProviderOrder(7, provider);
            var product = Product.MakeProduct("Test product", "Test product", 15, Dimensions.MakeDimensions(11, 12, 13));
            product.UpdateAmount(50);
            var providerOrderItem = ProviderOrderItem.MakeProviderOrderItem(providerOrder, product, 5);
            providerOrderItem.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => providerOrderItem.Delete());
        }
    }
}