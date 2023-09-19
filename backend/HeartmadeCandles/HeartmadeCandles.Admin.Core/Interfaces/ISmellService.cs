using HeartmadeCandles.Admin.Core.Models;

namespace HeartmadeCandles.Admin.Core.Interfaces;

public interface ISmellService
{
    Task<Smell[]> GetAll();
    Task<Smell> Get(int smellId);
    Task Create(Smell smell);
    Task Update(Smell smell);
    Task Delete(int smellId);
}