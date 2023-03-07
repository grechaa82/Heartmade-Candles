namespace HeartmadeCandles.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> IsVerify(string email, string password);
    }
}