using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests;

public class FeedbackRequest
{
    [Required]
    public FeedbackType Feedback { get; set; }
    [Required]
    public string UserName { get; set; }
}

public enum FeedbackType
{
    Telegram,
    Instagram,
    Whatsapp
}