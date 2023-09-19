namespace HeartmadeCandles.API.Contracts.Requests;

public class FeedbackRequest
{
    public FeedbackType Feedback { get; set; }
    public string UserName { get; set; }
}

public enum FeedbackType
{
    Telegram,
    Instagram,
    Whatsapp
}