using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<List<Decor>> GetDecorAsync();
        Task CreateDecorAsync(Decor decor);
        Task UpdateDecorAsync(Decor decor);
        Task DeleteDecorAsync(string id);
    }
}