using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCart
    {
        private string _id;
        private string _userId;
        private List<ShoppingCartItem> _items;
        private decimal _totalPrice;
        private string _description;

        [JsonConstructor]
        public ShoppingCart(
            string userId,
            List<ShoppingCartItem> items,
            string id = "",
            string description = "")
        {
            if (string.IsNullOrEmpty(userId)) 
            {  
                throw new ArgumentNullException($"'{nameof(userId)}' cannot be null or empty."); 
            }

            if (items == null)
            {
                throw new ArgumentNullException($"'{nameof(items)}' cannot be null.");
            }

            _id = id;
            _userId = userId;
            _items = items;
            _totalPrice = SetPrice(items);
            _description = description;
        }

        public string Id { get => _id; }
        public string UserId { get => _userId; }
        public List<ShoppingCartItem> Items { get => _items; }
        public decimal TotalPrice { get => _totalPrice; }
        public string Description { get => _description;}

        private decimal SetPrice (List<ShoppingCartItem> items)
        {
            var totalPrice = 0m;

            foreach (var item in items)
            {
                totalPrice += item.Price;
            }

            return totalPrice;
        }
    }
}
