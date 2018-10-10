using System;
using Xunit;
using Deposit.Models;

namespace Deposit_Tests
{
    public class CustomerTest
    {
        [Fact]
        public void MakeCustomer_EmptyName_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Customer.MakeCustomer(string.Empty, "75177495019", DateTime.Now));
            
            // assert
            Assert.Equal(e.Message, "Name can't be empty.");
        }

        [Fact]
        public void MakeCustomer_EmptyCpf_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Customer.MakeCustomer("Julio", string.Empty, DateTime.Now));
            
            // assert
            Assert.Equal(e.Message, "Cpf can't be empty.");
        }

        [Fact]
        public void MakeCustomer_InvalidCpf_ExceptionThrown()
        {
            // act
            var e = Assert.Throws<ArgumentException>(() => Customer.MakeCustomer("Julio", "75277495019", DateTime.Now));
            
            // assert
            Assert.Equal(e.Message, "Invalid Cpf.");
        }

        [Fact]
        public void MakeCustomer_ValidParameters_CustomerCreated()
        {
            // act
            var customer = Customer.MakeCustomer("Julio", "75177495019", DateTime.Now);
            
            // assert
            Assert.Equal(customer.Name, "Julio");
            Assert.Equal(customer.Cpf, "75177495019");
            Assert.Equal(customer.TotalSpent, 0);
        }

        [Fact]
        public void UpdateDocumentation_InvalidCpf_ExceptionThrown()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "75177495019", DateTime.Now);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customer.UpdateDocumentation(string.Empty, "75277495019"));
            
            // assert
            Assert.Equal(e.Message, "Invalid Cpf.");
        }

        [Fact]
        public void UpdateDocumentation_ValidParameters_ChangesNameAndCpf()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);
            
            // act
            customer.UpdateDocumentation("Leonardo", "99703100031");
            
            // assert
            Assert.Equal(customer.Name, "Leonardo");
            Assert.Equal(customer.Cpf, "99703100031");
        }

        [Fact]
        public void UpdateTotalSpent_Zero_ExceptionThrown()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customer.UpdateTotalSpent(0));
            
            // assert
            Assert.Equal(e.Message, "Value can't be equal to zero 0.");
        }

        [Fact]
        public void UpdateTotalSpent_PositiveValue_Updates()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);
            
            // act
            customer.UpdateTotalSpent(15);
            customer.UpdateTotalSpent(20);
            
            // assert
            Assert.Equal(customer.TotalSpent, 35);
        }

        [Fact]
        public void UpdateTotalSpent_NegativeValue_Updates()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);
            customer.UpdateTotalSpent(20);
            
            // act
            customer.UpdateTotalSpent(-15);
            
            // assert
            Assert.Equal(customer.TotalSpent, 5);
        }

        [Fact]
        public void UpdateTotalSpent_NegativeTotal_ExceptionThrown()
        {
            //arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);
            customer.UpdateTotalSpent(15);
            
            // act
            var e = Assert.Throws<ArgumentException>(() => customer.UpdateTotalSpent(-20));
            
            // assert
            Assert.Equal(e.Message, "Total spent can't be lower than 0.");
        }

        [Fact]
        public void CustomerDelete()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "10681541016", DateTime.Now);

            // act
            customer.Delete();

            // assert
            Assert.True(customer.IsDeleted);
        }

        [Fact]
        public void UpdateDocumentation_Customer_Deleted()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "75177495019", DateTime.Now);
            customer.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customer.UpdateDocumentation("Guilherme", "10681541016"));
        }

        [Fact]
        public void UpdateToalSpent_Customer_Deleted()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "75177495019", DateTime.Now);
            customer.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customer.UpdateTotalSpent(50));
        }

        [Fact]
        public void DeleteCustomer_DeletedCustomer_ExceptionThrown()
        {
            // arrange
            var customer = Customer.MakeCustomer("Julio", "75177495019", DateTime.Now);
            customer.Delete();

            // act
            var e = Assert.Throws<InvalidOperationException>(() => customer.Delete());
        }
    }
}