using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAdminService
    {
        #region Decor
        Task<List<Decor>> GetDecorAsync();
        Task CreateDecorAsync(Decor decor);
        Task UpdateDecorAsync(Decor decor);
        Task DeleteDecorAsync(string id);
        #endregion

        #region Smell
        Task<List<Smell>> GetSmellAsync();
        Task CreateSmellAsync(Smell smell);
        Task UpdateSmellAsync(Smell smell);
        Task DeleteSmellAsync(string id);
        #endregion
    }
}