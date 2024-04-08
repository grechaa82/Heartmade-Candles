using System.Text.Json.Serialization;

namespace HeartmadeCandles.UserAndAuth.Core.Models;

[Serializable]
public class TokenPayload
{
    [JsonPropertyName("userid")]
    public int UserId { get; init; }

    [JsonPropertyName("username")]
    public required string UserName { get; init; }

    [JsonPropertyName("role"), JsonConverter(typeof(JsonStringEnumConverter))]
    public Role Role { get; init; }

    [JsonPropertyName("sessionid")]
    public Guid SessionId { get; init; }
}
