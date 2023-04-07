
namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerPosition
    {
        UTG1 = 1,
        UTG2 = 2,
        MP1 = 3,
        MP2 = 4,
        LJ = 5,
        HJ = 6,
        CO = 7,
        BTN = 8,
        SB = 9,
        BB = 10
    }
}
