namespace HeartmadeCandles.Bot.Core.Models;

public enum OrderStatus
{
    Created,
    Confirmed,
    Placed,
    Paid,
    InProgress,
    Packed,
    InDelivery,
    Completed,
    Cancelled,
}
