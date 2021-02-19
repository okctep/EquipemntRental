using Domain;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class RentalPriceCalculationService
    {
        public Invoice GenerateInvoice(List<ConstructionEquipment> constructionEquipment)
        {
            var invoice = new Invoice() { Title = "Invoice " + DateTime.Now };
            var rentedEquipmentList = new List<RentedEquipment>();
            decimal totalPrice = 0;
            int loyaltyPoints = 0;

            foreach (var equip in constructionEquipment)
            {
                var rentedEquipment = new RentedEquipment();
                rentedEquipment.Name = equip.Name;
                rentedEquipment.Price = GetPrice(equip.Type, int.Parse(equip.RentalDays));
                totalPrice += rentedEquipment.Price;
                loyaltyPoints += (equip.Type == "Heavy") ? 2 : 1;
                rentedEquipmentList.Add(rentedEquipment);
            }
            invoice.RentedEquipment = rentedEquipmentList;
            invoice.PriceTotal = totalPrice;
            invoice.LoyaltyPoints = loyaltyPoints;

            return invoice;
        }

        public decimal GetPrice(string type, int rentalDays)
        {
            decimal price = 0;

            switch (type)
            {
                case "Heavy":
                    price = 100 + 60 * rentalDays;
                    break;
                case "Regular":
                    price = ((rentalDays <= 2) ? (60 * rentalDays) : (60 * 2 + (rentalDays - 2) * 40)) + 100;
                    break;
                case "Specialized":
                    price = (rentalDays <= 3) ? (60 * rentalDays) : (60 * 3 + (rentalDays - 3) * 40);
                    break;
                default:
                    Console.WriteLine("Unknown type");
                    break;
            }
            return price;
        }
    }
}
