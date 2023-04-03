namespace FlopClub.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        public List<Player> Players { get; set; } = new List<Player>(); 
        public decimal Pot { get; set; }
        public decimal SmallBlindValue { get; set; } = 5;
        public decimal BigBlindValue { get; set; } = 10;
        public int Dealer { get; set; }
        public List<Card> Deck { get; set; } = new List<Card>();
        public List<Card> Board { get; set; } = new List<Card>();
    }
}
