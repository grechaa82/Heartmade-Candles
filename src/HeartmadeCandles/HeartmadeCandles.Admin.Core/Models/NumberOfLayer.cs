namespace HeartmadeCandles.Admin.Core.Models
{
    public class NumberOfLayer
    {
        private int _id;
        private int _number;

        private NumberOfLayer(int id, int number)
        {
            _id = id;
            _number = number;
        }

        public int Id { get => _id; }
        public int Number { get => _number; }
        
        public static NumberOfLayer Create(int number, int id = 0)
        {
            if (number <= 0 || number >= 32)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(number)}' must be in the range from 0 to 32.");
            }
            
            return new NumberOfLayer(id, number);
        }
    }
}
