using System.ComponentModel.DataAnnotations;

namespace HeartmadeCandles.API.Contracts.Requests
{
    public class FeedbackRequest
    {
        public string TypeFeedback { get; set; }
        public string UserName { get; set; }
    }
}
