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

        #region Decor

        public async Task<List<Decor>> GetDecorAsync()
        {
            return await _adminRepository.GetDecorAsync();
        }

        public async Task CreateDecorAsync(Decor decor)
        {
            await _adminRepository.CreateDecorAsync(decor);
        }

        public async Task UpdateDecorAsync(Decor decor)
        {
            await _adminRepository.UpdateDecorAsync(decor);
        }

        public async Task DeleteDecorAsync(string id)
        {
            await _adminRepository.DeleteDecorAsync(id);
        }

        #endregion

        #region Smell

        public async Task<List<Smell>> GetSmellAsync()
        {
            return await _adminRepository.GetSmellAsync();
        }

        public async Task CreateSmellAsync(Smell smell)
        {
            await _adminRepository.CreateSmellAsync(smell);
        }

        public async Task UpdateSmellAsync(Smell smell)
        {
            await _adminRepository.UpdateSmellAsync(smell);
        }

        public async Task DeleteSmellAsync(string id)
        {
            await _adminRepository.DeleteSmellAsync(id);
        }

        #endregion
    }
}