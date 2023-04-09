namespace FlopClub.Services.PlayerService
{
    public interface IPlayerService
    {
        void Fold(Player player);
        void Check(Player player);
        void Call(Player player);
        void Raise(Player player);
    }
}
