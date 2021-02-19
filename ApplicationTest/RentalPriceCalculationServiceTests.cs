using Application.Services;
using Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApplicationTest
{
    public class RentalPriceCalculationServiceTests
    {
        private readonly RentalPriceCalculationService service;

        public RentalPriceCalculationServiceTests()
        {
            service = new RentalPriceCalculationService();
        }

        [Theory]
        [InlineData(new object[] { "Heavy", 5, 400 })]
        [InlineData(new object[] { "Regular", 5, 340 })]
        [InlineData(new object[] { "Regular", 2, 220 })]
        [InlineData(new object[] { "Specialized", 2, 120 })]
        [InlineData(new object[] { "Specialized", 6, 300 })]
        public void GetPrice_method_returns_valid_price(string type, int rentalDays, decimal expectedPrice)
        {
            //Act
            var price = service.GetPrice(type, rentalDays);

            //Assert
            Assert.Equal(expectedPrice, price);
        }

        [Fact]
        public void GenerateInvoice_method_returns_invoice_according_equipment()
        {
            //Arrange
            var constructionEquipmentList = new List<ConstructionEquipment>() {
              new ConstructionEquipment(){ Name = "Caterpillar bulldozer", RentalDays = "3", Type = "Heavy"},
              new ConstructionEquipment(){ Name = "KamAZ truck", RentalDays = "5", Type = "Regular"}
            };

            //Act
            var invoice = service.GenerateInvoice(constructionEquipmentList);

            //Assert
            Assert.Equal(620, invoice.PriceTotal);
            Assert.Equal(3, invoice.LoyaltyPoints);
            Assert.Equal(2, invoice.RentedEquipment.Count);
        }
    }
}
