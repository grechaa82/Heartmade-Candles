using HeartmadeCandles.Modules.Admin.Core.Interfaces;
using HeartmadeCandles.Modules.Admin.Core.Models;

namespace HeartmadeCandles.Modules.Admin.DAL.Repositories
{
    internal class CandleRepository : ICandleRepository
    {
        public Task Create(Candle candle)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Candle>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Candle>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Candle candle)
        {
            throw new NotImplementedException();
        }
    }
}
