﻿namespace HeartmadeCandles.Order.Core.Models;

public class Order
{
    public string? Id { get; init; }

    public required string BasketId { get; init; }

    public Basket? Basket { get; init; }

    public Feedback? Feedback { get; init; }

    public OrderStatus Status { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
}
