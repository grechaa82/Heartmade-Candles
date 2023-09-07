using System.Text.Json.Serialization;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class NumberOfLayerRequest
    {
        [JsonConstructor]
        public NumberOfLayerRequest(int number)
        {
            Number = number;
        }

        public int Number { get; set; }
    }
}
