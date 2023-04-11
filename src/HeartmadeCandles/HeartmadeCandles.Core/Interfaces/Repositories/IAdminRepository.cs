using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : ModelBase;
        Task CreateAsync<T>(T t) where T : ModelBase;
        Task UpdateAsync<T>(T t) where T : ModelBase;
        Task DeleteAsync<T>(T t) where T : ModelBase;
    }
}
