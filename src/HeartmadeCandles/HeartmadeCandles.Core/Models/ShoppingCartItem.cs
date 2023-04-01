namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCartItem : ModelBase
    {
        private ShoppingCartItem(
            string id,
            string userId,
            Candle candle,
            int quantity,
            decimal price)
        {
            Id = id;
            UserId = userId;
            Candle = candle;
            Quantity = quantity;
            Price = price;
        }

        public string UserId { get; }
        public Candle Candle { get; }
        public int Quantity { get; }
        public decimal Price { get; }

        public static (ShoppingCartItem, ErrorDetail[]) Create(
            string userId,
            Candle candle,
            int quantity,
            string id = null)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (candle == null)
            {
                errorsMessage = $"'{nameof(candle)}' connot be null.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (quantity <= 0)
            {
                errorsMessage = $"'{nameof(quantity)}' connot be zero or less than zero.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            // TODO: check TotalPrice

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var candlePrice = Candle.GetPrice(candle);
            var price = candlePrice * quantity;

            var cartItem = new ShoppingCartItem(
                id,
                userId,
                candle,
                quantity,
                price);

            return (cartItem, errors.ToArray());
        }

        public static (ShoppingCartItem, ErrorDetail[]) SetQuantity(ShoppingCartItem cartItem, int newQuantity)
        {
            var errors = new List<ErrorDetail>();
            var errorsMessage = string.Empty;

            if (cartItem == null || cartItem.Candle == null)
            {
                errorsMessage = $"'{nameof(cartItem)}' connot be null.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (newQuantity <= 0)
            {
                errorsMessage = $"'{nameof(newQuantity)}' connot be zero or less than zero.";
                errors.Add(new ErrorDetail(errorsMessage));
            }

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            var newCartItem = new ShoppingCartItem(
                cartItem.Id,
                cartItem.UserId,
                cartItem.Candle,
                newQuantity,
                cartItem.Price);

            return (newCartItem, errors.ToArray());
        }
    }
}
