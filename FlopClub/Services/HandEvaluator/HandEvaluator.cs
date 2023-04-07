namespace FlopClub.Services.HandEvaluator
{
    public class HandEvaluator : IHandEvaluator

    {
        public List<Player> EvaluateCardStrength(List<Player> players)
        {
            return players.OrderBy(player => player.Cards[0].Value)
                          .ThenByDescending(player => player.Cards[0].Suit)
                          .ToList();
        }
    }
}
