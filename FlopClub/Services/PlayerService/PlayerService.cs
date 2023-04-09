namespace FlopClub.Services.PlayerService
{
    public class PlayerService : IPlayerService
    {
        public void Call(Player player)
        {
            player.HasCalled = true;
        }

        public void Check(Player player)
        {
            player.HasChecked = true;
        }

        public void Fold(Player player)
        {
            player.HasFolded= true;
        }

        public void Raise(Player player)
        {
            player.HasRaised= true;
        }
    }
}
