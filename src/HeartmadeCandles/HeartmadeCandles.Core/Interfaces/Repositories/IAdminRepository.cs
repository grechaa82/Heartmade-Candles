using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<List<Decor>> GetDecorAsync();
        Task CreateDecorAsync(Decor decor);
        Task UpdateDecorAsync(Decor decor);
        Task DeleteDecorAsync(string id);
    }
}