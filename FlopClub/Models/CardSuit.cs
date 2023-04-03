
namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CardSuit
    {
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
        Spades = 4
    }
}
