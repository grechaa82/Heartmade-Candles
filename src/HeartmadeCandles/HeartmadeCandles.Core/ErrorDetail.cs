namespace HeartmadeCandles.Core
{
    public class ErrorDetail
    {
        public ErrorDetail(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
