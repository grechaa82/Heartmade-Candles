namespace HeartmadeCandles.Core.Models
{
    public class Decor : ModelBase
    {
        public bool IsUsed { get; set; }

        public string? Title { get; set; }

        public string? ImageURL { get; set; }

        public string? Description { get; set; }
    }
}
