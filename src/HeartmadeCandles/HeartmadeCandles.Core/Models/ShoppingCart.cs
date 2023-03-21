namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCart
    {
        private ShoppingCart(string id, List<ShoppingCartItem> items)
        {
            Id = id;
            Items = items;
        }

        public string Id { get; }
        public List<ShoppingCartItem> Items { get; }


        public static ShoppingCart Create(string id, List<ShoppingCartItem> shoppingCartItems)
        {


            var shoppingCart = new ShoppingCart(id, shoppingCartItems);

            return shoppingCart;
        }
    }
}