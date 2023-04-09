
namespace FlopClub.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public decimal Chips { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public HandStrength Hand { get; set; }
        public PlayerPosition Position { get; set; }
        public int TimeToAct { get; set; }
        public int BuyInCount { get; set; }
        public bool IsDealer { get; set; } = false;
        public bool HasMove { get; set; } = false;
        public bool HasFolded { get; set; } = false;
        public bool HasChecked { get; set; } = false;
        public bool HasCalled { get; set; } = false;
        public bool HasRaised { get; set; } = false;
        public decimal AmmountToCall { get; set; }
        public decimal CurrentBet { get; set; }
        public PlayerAction PlayerAction { get; set; }
        public decimal RaisedTo { get; set; }
    }
}
