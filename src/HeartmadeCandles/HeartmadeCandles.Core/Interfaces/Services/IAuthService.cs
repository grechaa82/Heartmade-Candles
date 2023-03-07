namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> IsVerifyAsync(string email, string password);
        Task<bool> RegisterUserAsync(string nickName, string email, string password);
    }
}