namespace FlopClub.Services.PlayerService
{
    public interface IPlayerService
    {
        void Fold(Player player);
        void Check(Player player);
        void Call(Player player, decimal ammountToCall);
        void Raise(Player player, decimal raiseAmmount);
        void EndMove(Player player);
        void ClearValues(Player player);
    }
}
