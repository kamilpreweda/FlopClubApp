
namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CardSuit
    {
        Hearts = 4,
        Spades = 3,
        Diamonds = 2,
        Clubs = 1
    }
}
