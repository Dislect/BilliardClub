namespace BilliardClub.Models
{
    public class CartFoodItem
    {
        public int id { get; set; }
        public FoodItem FoodItem { get; set; }
        public uint quantity { get; set; }
        public string cartId { get; set; }
    }
}
