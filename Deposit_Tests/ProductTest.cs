using Deposit.Models;
using System;
using Xunit;

namespace Deposit_Tests
{
    public class ProductTest
    {
        [Fact]
        public void MakeProduct_EmptyName_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // act
            var e = Assert.Throws<ArgumentException>(() => Product.MakeProduct(string.Empty, "Test product", 50, dimensions));

            // assert
            Assert.Equal(e.Message, "Name can't be empty.");
        }

        [Fact]
        public void MakeProduct_EmptyDescription_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // act
            var e = Assert.Throws<ArgumentException>(() => Product.MakeProduct("Test", string.Empty, 50, dimensions));

            // assert
            Assert.Equal(e.Message, "Description can't be empty.");
        }

        [Fact]
        public void MakeProduct_ZeroPrice_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // act
            var e = Assert.Throws<ArgumentException>(() => Product.MakeProduct("Test", "Test product", 0, dimensions));

            // assert
            Assert.Equal(e.Message, "Price must be larger than 0.");
        }

        [Fact]
        public void MakeProduct_NegativePrice_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // act
            var e = Assert.Throws<ArgumentException>(() => Product.MakeProduct("Test", "Test product", -10, dimensions));

            // assert
            Assert.Equal(e.Message, "Price must be larger than 0.");
        }

        [Fact]
        public void MakeProduct_NullDimensions_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentNullException>(() => Product.MakeProduct("Test", "Test product", 50, null));
        }

        [Fact]
        public void MakeProduct_ValidParameters_ProductCreated()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // act
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // assert
            Assert.Equal(product.Name, "Test");
            Assert.Equal(product.Description, "Test product");
            Assert.Equal(product.Amount, 0);
            Assert.Equal(product.Price, 50);
            Assert.Equal(product.Sku, "TES-PRO-11-12-13");
        }

        [Fact]
        public void Up()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.UpdatePrice(0));

            // assert
            Assert.Equal(e.Message, "Price must be larger than 0.");
        }

        [Fact]
        public void UpdatePrice_NegativePrice_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.UpdatePrice(-10));

            // assert
            Assert.Equal(e.Message, "Price must be larger than 0.");
        }

        [Fact]
        public void UpdatePrice_PositivePrice_PriceUpdated()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            product.UpdatePrice(20);

            // assert
            Assert.Equal(product.Price, 20);
        }

        [Fact]
        public void UpdateAmount_ZeroAmount_AmountUpdated()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.UpdateAmount(0));

            // assert
            Assert.Equal(e.Message, "Amount can't be equal to 0.");
        }

        [Fact]
        public void UpdateAmount_PositiveAmount_AmountUpdated()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            product.UpdateAmount(20);
            product.UpdateAmount(15);

            // assert
            Assert.Equal(product.Amount, 35);
        }

        [Fact]
        public void UpdateAmount_NegativeAmount_AmountUpdated()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            product.UpdateAmount(20);
            product.UpdateAmount(-15);

            // assert
            Assert.Equal(product.Amount, 5);
        }

        [Fact]
        public void UpdateAmount_NegativeTotal_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.UpdateAmount(-15));

            // assert
            Assert.Equal(e.Message, "Total amount can't be lower than 0.");
        }

        [Fact]
        public void RenameProduct_EmptyName_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.Rename(string.Empty, "Product test"));

            // assert
            Assert.Equal(e.Message, "Name can't be empty.");
        }

        [Fact]
        public void RenameProduct_EmptyDescription_ExceptionThrown()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            var e = Assert.Throws<ArgumentException>(() => product.Rename("Product", string.Empty));

            // assert
            Assert.Equal(e.Message, "Description can't be empty.");
        }

        [Fact]
        public void RenameProduct_ValidParameters_ProductRenamed()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            product.Rename("Product", "Product test");

            // assert
            Assert.Equal(product.Name, "Product");
            Assert.Equal(product.Description, "Product test");
            Assert.Equal(product.Sku, "PRO-TES-11-12-13");
        }

        [Fact]
        public void RedimensionProduct_ValidParameters_ProductRedimensioned()
        {
            // arrange
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            // act
            product.Redimension(14, 15, 16);

            // assert
            Assert.Equal(dimensions.Width, 14);
            Assert.Equal(dimensions.Height, 15);
            Assert.Equal(dimensions.Depth, 16);
            Assert.Equal(product.Sku, "TES-PRO-14-15-16");
        }

        [Fact]
        public void ProductDelete()
        {
            // arrange 
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);
            var product = Product.MakeProduct("Test", "Test product", 50, dimensions);

            //act
            product.Delete();

            // assert
            Assert.True(product.IsDeleted);

        }
    }

    
}