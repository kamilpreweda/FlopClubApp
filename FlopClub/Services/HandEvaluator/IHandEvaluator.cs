namespace FlopClub.Services.HandEvaluator
{
    public interface IHandEvaluator
    {
        List<Player> EvaluateCardStrength(List<Player> players);
    }
}
