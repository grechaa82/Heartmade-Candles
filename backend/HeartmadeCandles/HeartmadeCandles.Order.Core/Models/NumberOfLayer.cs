namespace HeartmadeCandles.Order.Core.Models
{
    public class NumberOfLayer
    {
        public NumberOfLayer(int id, int number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; private set; }
        public int Number { get; private set; }
    }
}