namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCart
    {
        private ShoppingCart(
            string id,
            string userId,
            List<ShoppingCartItem> items,
            decimal totalPrice)
        {
            Id = id;
            UserId = userId;
            Items = items;
            TotalPrice = totalPrice;
        }

        public string? Id { get; set; }
        public string UserId { get; }
        public List<ShoppingCartItem> Items { get; }
        public decimal TotalPrice { get; }


        public static ShoppingCart Create(
            string userId,
            List<ShoppingCartItem> shoppingCartItems,
            string id = null)
        {
            var totalPrice = 0m;
            foreach (var item in shoppingCartItems)
            {
                totalPrice += item.Price;
            }

            var shoppingCart = new ShoppingCart(
                id,
                userId,
                shoppingCartItems,
                totalPrice);

            return shoppingCart;
        }
    }
}
