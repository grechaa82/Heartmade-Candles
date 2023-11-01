using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class FeedbackMapping
{
    public static Feedback MapToFeedback(FeedbackCollection feedbackCollection)
    {
        return new Feedback(
            feedbackCollection.TypeFeedback,
            feedbackCollection.UserName);
    }

    public static FeedbackCollection MapToFeedbackCollection(Feedback feedback)
    {
        return new FeedbackCollection
        {
            TypeFeedback = feedback.TypeFeedback,
            UserName = feedback.UserName
        };
    }
}

