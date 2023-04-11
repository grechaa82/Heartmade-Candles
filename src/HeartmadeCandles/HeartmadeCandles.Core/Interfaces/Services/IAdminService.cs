using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<List<T>> GetAllAsync<T>() where T : ModelBase;
        Task CreateAsync<T>(T t) where T : ModelBase;
        Task UpdateAsync<T>(T t) where T : ModelBase;
        Task DeleteAsync<T>(T t) where T : ModelBase;
    }
}
