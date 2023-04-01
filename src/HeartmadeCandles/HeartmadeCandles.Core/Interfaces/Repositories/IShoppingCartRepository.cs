﻿using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<List<ShoppingCartItem>> GetByUserIdAsync(string userId);
    }
}
