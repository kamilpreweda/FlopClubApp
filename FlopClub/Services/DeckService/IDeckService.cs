namespace FlopClub.Services.DeckService
{
    public interface IDeckService
    {
        void PopulateDeck(Game game);
        void ShuffleDeck(List<Card> deck);
        void DealCards(Game game);
    }
}
