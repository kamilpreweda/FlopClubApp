namespace FlopClub.Services.TableService
{
    public class TableService : ITableService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IDeckService _deckService;
        private readonly IHandEvaluator _handEvaluator;

        public TableService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IDeckService deckService, IHandEvaluator handEvaluator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _deckService = deckService;
            _handEvaluator = handEvaluator;
        }
        private User GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext!.User!.Identity!.Name;
            var user = _context.Users
                .Include(u => u.Lobbies)
                .Include(u => u.UserRoles!).ThenInclude(ur => ur.Role)
                .SingleOrDefault(u => u.Username == username);

            return user!;
        }
        private bool CanJoin(Game game)
        {
            if (game.ActivePlayers >= game.MaxPlayers)
            {
                return false;
            }
            return true;
        }

        public async Task<ServiceResponse<GetGameDto>> BuyIn(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games
                .Include(g => g.Players)
                .Include(g => g.Lobby).ThenInclude(l => l.Users)
                .SingleOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            if(!game.Lobby.Users.Any(u => u.Id == user.Id))
            {
                response.Success = false;
                response.Message = "To join game, user must be first in it's lobby.";
                return response;
            }

            if (game!.Players.Any(p => p.UserId == user.Id))
            {
                response.Success = false;
                response.Message = "User is already a player in the game.";
                return response;
            }

            if (!CanJoin(game))
            {
                response.Success = false;
                response.Message = "Game is full";
                return response;
            }

            var availablePositions = Enum.GetValues(typeof(PlayerPosition))
                .Cast<PlayerPosition>()
                .Where(p => !game.Players.Any(x => x.Position == p))
                .ToList();

            var player = new Player
            {
                UserId = user.Id,
                GameId = game.Id,
                Chips = 1000,
                Position = availablePositions.First()
            };

            game.Players.Add(player);
            game.ActivePlayers++;
            player.BuyInCount++;
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Player was added to the game.";
            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> LeaveTable(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games.SingleOrDefaultAsync(g => g.Id == gameId);
            var player = game!.Players.FirstOrDefault(p => p.UserId == user.Id);

            game.Players.Remove(player!);
            game.ActivePlayers--;
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Player was removed from the table";
            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> SetPositions(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games
                .Include(g => g.Players)
                .SingleOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            var isAdmin = user.UserRoles!.Any(ur => ur.Role.Name == "GameAdmin");

            if (!isAdmin)
            {
                response.Success = false;
                response.Message = "User is not an admin of the game";
                return response;
            }

            if (game.Players.Count < 2)
            {
                response.Success = false;
                response.Message = "At least two players are required to set positions";
                return response;
            }

            _deckService.PopulateDeck(game);

            foreach (var player in game.Players)
            {
                player.Cards.Add(game.Deck[0]);
                game.Deck.RemoveAt(0);
            }

            var sortedPlayers = _handEvaluator.EvaluateCardStrength(game.Players);
            game.Players = sortedPlayers;
            int bigBlindPositionIndex = (int)PlayerPosition.BB;

            foreach (var player in game.Players)
            {
                player.Position = (PlayerPosition)bigBlindPositionIndex;
                bigBlindPositionIndex--;
            }

            foreach (var player in game.Players)
            {
                foreach (var card in player.Cards)
                {
                    game.Deck.Add(card);
                }
                player.Cards.Clear();
            }

            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> StartGame(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games.FindAsync(gameId);

            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            var isAdmin = user.UserRoles.Any(ur => ur.Role.Name == "GameAdmin");

            if (!isAdmin)
            {
                response.Success = false;
                response.Message = "User is not an admin of the game";
                return response;
            }

            game.IsRunning = true;
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> PauseGame(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games.FindAsync(gameId);

            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            var isAdmin = user.UserRoles.Any(ur => ur.Role.Name == "GameAdmin");

            if (!isAdmin)
            {
                response.Success = false;
                response.Message = "User is not an admin of the game";
                return response;
            }

            game.IsRunning = false;
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }
    }
}
