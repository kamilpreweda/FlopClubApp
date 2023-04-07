
namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerPosition
    {
        BTN = 1,
        CO = 2,
        HJ = 3,
        LJ = 4,
        MP2 = 5,
        MP1 = 6,
        UTG2 = 7,
        UTG1 = 8,
        SB = 9,
        BB = 10
    }
}
