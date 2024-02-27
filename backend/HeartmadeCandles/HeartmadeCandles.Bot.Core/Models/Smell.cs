﻿namespace HeartmadeCandles.Bot.Core.Models;

public class Smell
{
    public int Id { get; init; }

    public required string Title { get; init; }

    public required decimal Price { get; init; }
}