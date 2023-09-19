using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface IDecorService
{
    Task<Decor[]> GetAll();
    Task<Decor> Get(int decorId);
    Task Create(Decor decor);
    Task Update(Decor decor);
    Task Delete(int decorId);
}