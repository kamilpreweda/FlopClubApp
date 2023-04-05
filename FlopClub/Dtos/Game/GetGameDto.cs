namespace FlopClub.Dtos.Game
{
    public class GetGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new List<Player>();
        public int MaxPlayers { get; set; } = 10;
        public int ActivePlayers { get; set; } = 0;
        public int LobbyId { get; set; }

    }
}
