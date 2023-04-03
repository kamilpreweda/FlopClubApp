namespace FlopClub.Models
{
    public class Card
    {
        public int Id { get; set; }
        public CardValue Value { get; set; }
        public CardSuit Suit { get; set; }
    }
}
