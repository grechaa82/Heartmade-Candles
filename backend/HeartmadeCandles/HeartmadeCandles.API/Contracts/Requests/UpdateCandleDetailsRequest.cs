namespace HeartmadeCandles.API.Contracts.Requests
{
    public class UpdateCandleDetailsRequest
    {
        public CandleRequest CandleRequest { get; set; }
        public List<int> DecorsIds { get; set; }
        public List<int> LayerColorsIds { get; set; }
        public List<int> NumberOfLayersIds { get; set; }
        public List<int> SmellsIds { get; set; }
        public List<int> WicksIds { get; set; }
    }
}
