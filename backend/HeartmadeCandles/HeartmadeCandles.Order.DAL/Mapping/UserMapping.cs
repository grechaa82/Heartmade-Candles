using HeartmadeCandles.Order.Core.Models;
using HeartmadeCandles.Order.DAL.Documents;

namespace HeartmadeCandles.Order.DAL.Mapping;

internal class UserMapping
{
    public static User MapToUser(UserDocument userDocument)
    {
        return new User(
            userDocument.FirstName,
            userDocument.LastName,
            userDocument.Phone,
            userDocument.Email);
    }

    public static UserDocument MapToUserDocument(User user)
    {
        return new UserDocument
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
        };
    }
}

