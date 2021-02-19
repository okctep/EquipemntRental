using System.Collections.Generic;

namespace Domain
{
    public class Invoice
    {
        public string Title { get; set; }
        public List<RentedEquipment> RentedEquipment { get; set; }
        public decimal PriceTotal { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class RentedEquipment
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
