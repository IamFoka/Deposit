using Deposit.Domain.Entities;
using System;
using Xunit;

namespace Deposit_Tests
{
    public class DimensionsTest
    {
        [Fact]
        public void MakeDimensions_ZeroWidth_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Dimensions.MakeDimensions(0, 12, 13));

            // assert
            Assert.Equal(e.Message, "Width must be larger than 0.");
        }

        [Fact]
        public void MakeDimensions_ZeroHeight_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Dimensions.MakeDimensions(11, 0, 13));

            // assert
            Assert.Equal(e.Message, "Height must be larger than 0.");
        }

        [Fact]
        public void MakeDimensions_ZeroDepth_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Dimensions.MakeDimensions(11, 12, 0));

            // assert
            Assert.Equal(e.Message, "Depth must be larger than 0.");
        }

        [Fact]
        public void MakeDimensions_ValidParameters_DimensionsCreated()
        {
            // act
            var dimensions = Dimensions.MakeDimensions(11, 12, 13);

            // assert
            Assert.Equal(dimensions.Width, 11);
            Assert.Equal(dimensions.Height, 12);
            Assert.Equal(dimensions.Depth, 13);
        }
    }
}