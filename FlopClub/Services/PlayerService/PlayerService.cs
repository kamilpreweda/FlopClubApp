namespace FlopClub.Services.PlayerService
{
    public class PlayerService : IPlayerService
    {
        public void Call(Player player, decimal ammountToCall)
        {
            player.Chips -= ammountToCall;
            player.CurrentBet = ammountToCall;
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

        public void Raise(Player player, decimal raiseAmmount)
        {
            if (raiseAmmount <= 20)
            {
                throw new ArgumentException("Raise amount must be greater or equal to (2xBB).", nameof(raiseAmmount));
            }

            if(raiseAmmount > player.Chips)
            {
                throw new ArgumentException("The player does not have enough chips to make this raise.");
            }

            player.RaisedTo = raiseAmmount;
            player.Chips -= raiseAmmount;
            player.CurrentBet = raiseAmmount;
            player.HasRaised= true;
        }

        public void EndMove(Player player)
        {
            player.HasMove = false;
        }

        public void ClearValues(Player player)
        {
            player.CurrentBet = 0;
            player.AmmountToCall = 0;
            player.RaisedTo = 0;
            player.HasChecked = false;
            player.HasCalled = false;
            player.HasFolded = false;
            player.HasRaised = false;
            player.HasMove = false;
        }
    }
}
