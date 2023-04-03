
namespace FlopClub.Models
{
    public class Player
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public decimal Chips { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public HandStrength Hand { get; set; }
        public PlayerPosition Position { get; set; }
        public int TimeToAct { get; set; }
    }
}
