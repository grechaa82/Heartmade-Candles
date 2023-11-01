using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Collections;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class UserMapping
{
    public static User MapToUser(UserCollection userCollection)
    {
        return new User(
            userCollection.FirstName,
            userCollection.LastName,
            userCollection.Phone,
            userCollection.Email);
    }

    public static UserCollection MapToUserCollection(User user)
    {
        return new UserCollection
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
        };
    }
}

