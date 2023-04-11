using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.Core.Models;

namespace HeartmadeCandles.BusinessLogic.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<T>> GetAllAsync<T>() where T : ModelBase
        {
            var result = new List<T>();
            var items =  await _adminRepository.GetAllAsync<T>();
            foreach (var item in items) 
            { 
                result.Add(item);
            }
            return result;
        }

        public async Task CreateAsync<T>(T t) where T : ModelBase
        {
            await _adminRepository.CreateAsync(t);
        }

        public async Task UpdateAsync<T>(T t) where T : ModelBase
        {
            await _adminRepository.UpdateAsync(t);
        }

        public async Task DeleteAsync<T>(T t) where T : ModelBase
        {
            await _adminRepository.DeleteAsync(t);
        }
    }
}
