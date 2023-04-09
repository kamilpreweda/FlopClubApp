namespace FlopClub.Services.GameLogicService
{
    public interface IGameLogicService
    {
        void SetFirstPlayerMove(Game game);

        List<PlayerAction> DefinePossibleActions(Game game, Player player);

        void SetAmmountToCall(decimal amountToCall, Player player);

        void HandleAction(Game game, Player player, PlayerAction action);
    }
}
