using FlopClub.Models;
using System;

namespace FlopClub.Services.DeckService
{
    public class DeckService : IDeckService
    {
        private readonly Random _random = new Random();

        public void PopulateDeck(Game game)
        {
            game.Deck = new List<Card>();

            foreach (var suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (var value in Enum.GetValues(typeof(CardValue)))
                {
                    game.Deck.Add(new Card { Suit = (CardSuit)suit, Value = (CardValue)value });
                }
            }

            ShuffleDeck(game.Deck);
        }

        public void ShuffleDeck(List<Card> deck)
        {
            for(int i = deck.Count - 1; i > 0; i--)
        {
                int j = _random.Next(0, i + 1);
                var temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }
        }

        public void DealCards(Game game)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var player in game.Players)
                {
                    player.Cards.Add(game.Deck.First());
                    game.Deck.Remove(game.Deck.First());
                }
            }
        }
    }
}
