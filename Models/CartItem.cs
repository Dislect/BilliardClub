namespace BilliardClub.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public PoolTable PoolTable { get; set; }
        public FoodItem FoodItem { get; set; }
        public uint quantity { get; set; }
        public string cartId { get; set; }
    }
}
