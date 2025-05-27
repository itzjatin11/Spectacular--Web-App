namespace SpecTacular.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Title { get; set; } // e.g., "30% off on Purchases over $50"
        public string Description { get; set; } // e.g., "Enjoy a fantastic 30% discount..."
        public decimal DiscountPercentage { get; set; } // e.g., 30 for 30%
        public decimal MinPurchaseAmount { get; set; } // e.g., 50 for $50
        public DateTime? ValidUntil { get; set; } // Optional expiry date
        public bool IsActive { get; set; } // To enable/disable promotions
    }
}
