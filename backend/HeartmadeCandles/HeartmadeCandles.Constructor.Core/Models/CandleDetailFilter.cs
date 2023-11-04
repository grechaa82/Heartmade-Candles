﻿namespace HeartmadeCandles.Constructor.Core.Models;

public class CandleDetailFilter
{
    public int CandleId { get; init; }

    public int? DecorId { get; init; }

    public int NumberOfLayerId { get; init; }

    public required int[] LayerColorIds { get; init; }

    public int? SmellId { get; init; }

    public int WickId { get; init; }
}
