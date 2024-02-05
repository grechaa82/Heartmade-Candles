namespace HeartmadeCandles.Bot;

public class TelegramUser
{
    public TelegramUser(
        long userId, 
        long chatId,
        string? userName, 
        string firstName = "", 
        string lastName = "",
        string currentOrderId = "",
        TelegramUserState state = TelegramUserState.None,
        TelegramUserRole role = TelegramUserRole.Buyer)
    {
        UserId = userId;
        ChatId = chatId;
        UserName = userName ?? $"{firstName}{(string.IsNullOrEmpty(lastName) ? string.Empty : " " + lastName)}";
        FirstName = firstName;
        LastName = lastName;
        CurrentOrderId = currentOrderId;
        State = state;
        Role = role;
    }

    public long UserId { get; init; }

    public long ChatId { get; init; }

    public string? UserName {  get; init; }
    
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? CurrentOrderId { get; init; }

    public TelegramUserState State { get; init; }

    public TelegramUserRole Role { get; init; }

    public TelegramUser UpdateState(TelegramUserState newState)
    {
        return new TelegramUser(
            UserId, 
            ChatId, 
            UserName, 
            FirstName, 
            LastName, 
            CurrentOrderId, 
            newState, 
            Role);
    }
}
