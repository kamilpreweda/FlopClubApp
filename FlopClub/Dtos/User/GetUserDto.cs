namespace FlopClub.Dtos.User
{
    public class GetUserDto
    {
        public string Username { get; set; } = string.Empty;
        public int HandsPlayed { get; set; }
        public decimal TotalWinnings { get; set; }
        public List<int> LobbiesIds { get; set; } = new List<int>();
    }
}
