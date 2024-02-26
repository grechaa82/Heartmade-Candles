namespace HeartmadeCandles.Bot.BL.Utilities;

public class TelegramMessageFormatter
{
    public static string Format(string input)
    {
        var specialCharacters = new[]
        {
            "_", "*", "[", "]", "(", ")", "~", "`", ">",
            "#", "+", "-", "=", "|", "{", "}", ".", "!"
        };

        foreach (var specialCharacter in specialCharacters)
        {
            input = input.Replace(specialCharacter, "\\" + specialCharacter);
        }

        return input;
    }
}