
namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerPosition
    {
        D = 1,
        CO = 2,
        HJ = 3,
        MP3 = 4,
        MP2 = 5,
        MP1 = 6,
        UTG3 = 7,
        UTG2 = 8,
        UTG1 = 9,
        UTG = 10
    }
}
