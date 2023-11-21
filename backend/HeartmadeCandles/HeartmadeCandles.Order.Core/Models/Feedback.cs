namespace HeartmadeCandles.Order.Core.Models;

public class Feedback
{
    public Feedback(string typeFeedback, string userName)
    {
        TypeFeedback = typeFeedback;
        UserName = userName;
    }

    public string TypeFeedback { get; private set; }

    public string UserName { get; private set; }
}