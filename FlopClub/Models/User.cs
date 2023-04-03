namespace FlopClub.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        public int HandsPlayed { get; set; }
        public decimal TotalWinnings { get; set; }
    }
}
