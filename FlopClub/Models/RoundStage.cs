namespace FlopClub.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoundStage
    {
        Preflop = 1,
        Flop = 2,
        Turn = 3,
        River = 4,
        Showdown = 5
    }
}
