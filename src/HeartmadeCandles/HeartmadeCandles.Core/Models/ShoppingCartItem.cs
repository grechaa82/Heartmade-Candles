using System.Text.Json.Serialization;

namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCartItem : ModelBase
    {
        private string _userId;
        private Candle _candle;
        private int _quantity;
        private decimal _price;

        [JsonConstructor]
        public ShoppingCartItem(
            string userId,
            Candle candle, 
            int quantity,
            decimal price,
            string id = "")
        {
            if (candle == null)
            {
                throw new ArgumentNullException($"'{nameof(candle)}' connot be null.");
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(quantity)}' connot be zero or less than zero.");
            }

            // TODO: check TotalPrice

            Id = id;
            _userId = userId;
            _candle = candle;
            _quantity = quantity;
            _price  = price;
        }

        public string UserId { get => _userId;  }
        public Candle Candle { get => _candle;  }
        public int Quantity { get => _quantity;  }
        public decimal Price { get => _price;  }

        public static ShoppingCartItem SetQuantity(ShoppingCartItem cartItem, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentNullException($"'{nameof(newQuantity)}' connot be zero or less than zero.");
            }

            var newCartItem = new ShoppingCartItem(
                cartItem.UserId,
                cartItem.Candle, 
                newQuantity,
                cartItem.Price,
                cartItem.Id);

            return newCartItem;
        }
    }
}
