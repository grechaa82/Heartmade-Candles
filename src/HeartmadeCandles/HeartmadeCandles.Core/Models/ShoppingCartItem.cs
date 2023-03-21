namespace HeartmadeCandles.Core.Models
{
    public class ShoppingCartItem
    {
        private ShoppingCartItem(Candle candle, int quantity)
        {
            Candle = candle;
            Quantity = quantity;
        }

        public Candle Candle { get; }
        public int Quantity { get; }
        public decimal TotalAmount { get; }

        public static (ShoppingCartItem, ErrorDetail[]) Create(Candle candle, int quantity)
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

            // TODO: check TotalAmount

            if (errors.Any())
            {
                return (null, errors.ToArray());
            }

            // TODO: Set TotalAmount (price Candle * Quantity)

            var cartItem = new ShoppingCartItem(candle, quantity);

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

            var newCartItem = new ShoppingCartItem(cartItem.Candle, newQuantity);

            return (newCartItem, errors.ToArray());
        }
    }
}
