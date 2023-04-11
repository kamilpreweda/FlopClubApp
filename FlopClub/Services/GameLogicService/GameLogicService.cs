using FlopClub.Services.PlayerService;

namespace FlopClub.Services.GameLogicService
{
    public class GameLogicService : IGameLogicService
    {
        private readonly IPlayerService _playerService;

        public GameLogicService(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        public void SetFirstPlayerMove(Game game)
        {
            foreach (var player in game.Players)
            {
                player.HasMove = false;
            }

            if (game.RoundStage == RoundStage.Preflop)
            {
                if (game.Players.Count == 2)
                {
                    game.Players.SingleOrDefault(p => p.Position == PlayerPosition.SB)!.HasMove = true;
                }

                game.Players.SingleOrDefault(p => p.Position == PlayerPosition.SB - 1)!.HasMove = true;
            }

            if (game.RoundStage == RoundStage.Flop || game.RoundStage == RoundStage.Turn || game.RoundStage == RoundStage.River)
            {
                if (game.Players.Count == 2)
                {
                    game.Players.SingleOrDefault(p => p.Position == PlayerPosition.SB)!.HasMove = true;
                }

                game.Players.SingleOrDefault(p => p.Position == PlayerPosition.SB - 1)!.HasMove = true;
            }

            if(game.RoundStage == RoundStage.Showdown)
            {
                return;
            }
        }

        public List<PlayerAction> DefinePossibleActions(Game game, Player player)
        {
            var possibleActions = new List<PlayerAction>();

            if (player.HasFolded)
            {
                return possibleActions;
            }

            possibleActions.Add(PlayerAction.Fold);

            if (player.AmmountToCall == 0)
            {
                possibleActions.Add(PlayerAction.Check);
            }

            if (player.CurrentBet < game.CurrentBet)
            {
                possibleActions.Add(PlayerAction.Call);
            }

            if(player.Chips > game.CurrentBet)
            {
                possibleActions.Add(PlayerAction.Raise);
            }

            return possibleActions;
        }

        public void SetAmmountToCall(decimal amountToCall, Player player) 
        {
            player.AmmountToCall = amountToCall;
        }

        public void HandleAction(Game game, Player player, PlayerAction action, decimal raiseAmmount = 0)
        {
            switch (action)
            {
                case PlayerAction.Fold: 
                    {
                        _playerService.Fold(player);
                    }
                    break;

                case PlayerAction.Check: 
                    {
                        _playerService.Check(player);
                    }
                    break;

                case PlayerAction.Call: 
                    {
                        game.Pot += player.AmmountToCall;
                        _playerService.Call(player, player.AmmountToCall);
                    }
                    break;

                case PlayerAction.Raise: 
                    {                   
                        game.Pot += raiseAmmount;
                        game.CurrentBet = raiseAmmount;
                        _playerService.Raise(player, raiseAmmount);
                    }
                    break;
            }

            _playerService.EndMove(player);
        }
    }
}
