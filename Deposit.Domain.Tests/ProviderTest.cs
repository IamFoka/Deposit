using System;
using Xunit;
using Deposit.Domain.Entities;

namespace Deposit_Tests
{
    public class ProviderTest
    {
        [Fact]
        public void MakeProvider_EmptyName_ExceptionThrown()
        {
            // act
            Assert.Throws<ArgumentException>(() => Provider.MakeProvider(string.Empty, "17475801000106"));
        }

        [Fact]
        public void MakeProvider_EmptyCnpj_ExceptionThrown()
        {
            // act
            Assert.Throws<ArgumentException>(() => Provider.MakeProvider("Julio", string.Empty));
        }

        [Fact]
        public void MakeProvider_InvalidCnpj_ExceptionThrown()
        {
            // act
            Assert.Throws<ArgumentException>(() => Provider.MakeProvider("Julio", "17475801000107"));
        }

        [Fact]
        public void MakeProvider_ValidParameters_CustomerCreated()
        {
            // act
            var provider = Provider.MakeProvider("Julio", "17475801000106");
            
            // assert
            Assert.Equal(provider.Name, "Julio");
            Assert.Equal(provider.Cnpj, "17475801000106");
        }

        [Fact]
        public void UpdateDocumentation_InvalidCnpj_ExceptionThrown()
        {
            //arrange
            var provider = Provider.MakeProvider("Julio", "17475801000106");
            
            // act
            Assert.Throws<ArgumentException>(() => provider.UpdateDocumentation(string.Empty, "17475801000107"));
        }

        [Fact]
        public void UpdateDocumentation_ValidParameters_ChangesNameAndCnpj()
        {
            //arrange
            var provider = Provider.MakeProvider("Julio", "17475801000106");
            
            // act
            provider.UpdateDocumentation("Leonardo", "09883060000174");
            
            // assert
            Assert.Equal(provider.Name, "Leonardo");
            Assert.Equal(provider.Cnpj, "09883060000174");
        }

        [Fact]
        public void DeleteProvider_ValidParameters_ProviderDeleted()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "17475801000106");

            // act
            provider.Delete();

            // assert
            Assert.True(provider.IsDeleted);
        }

        [Fact]
        public void DeleteProvider_ProviderDeleted_ExceptionThrown()
        {
            // arrange
            var provider = Provider.MakeProvider("Julio", "17475801000106");
            provider.Delete();

            // act
            Assert.Throws<InvalidOperationException>(() => provider.Delete());
        }
    }
}