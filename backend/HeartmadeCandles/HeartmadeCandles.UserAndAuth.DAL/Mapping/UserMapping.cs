using HeartmadeCandles.UserAndAuth.Core.Models;
using HeartmadeCandles.UserAndAuth.DAL.Entities;

namespace HeartmadeCandles.UserAndAuth.DAL.Mapping;

internal class UserMapping
{
    public static User MapToUser(UserEntity userEntity)
    {
        var user = User.Create(
            id: userEntity.Id,
            email: userEntity.Email,
            userName: userEntity.UserName,
            passwordHash: userEntity.PasswordHash,
            registrationDate: userEntity.RegistrationDate);

        return user.Value;
    }

    public static UserEntity MapToUserEntity(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            RegistrationDate = user.RegistrationDate
        };

        return userEntity;
    }
}
