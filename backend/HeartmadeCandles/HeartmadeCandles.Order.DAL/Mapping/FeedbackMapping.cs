using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class FeedbackMapping
{
    public static Feedback MapToFeedback(FeedbackDocument feedbackDocument)
    {
        return new Feedback(
            feedbackDocument.TypeFeedback,
            feedbackDocument.UserName);
    }

    public static FeedbackDocument MapToFeedbackDocument(Feedback feedback)
    {
        return new FeedbackDocument
        {
            TypeFeedback = feedback.TypeFeedback,
            UserName = feedback.UserName
        };
    }
}

