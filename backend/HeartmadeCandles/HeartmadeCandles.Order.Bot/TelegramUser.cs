namespace HeartmadeCandles.Order.Bot;

internal class TelegramUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public long? ChatId { get; set; }

    public string? UserName {  get; set; }
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}
