using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FlopClub.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly DataContext _context;
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameService(DataContext context, IEncrypter encrypter, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _encrypter = encrypter;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private User GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext!.User!.Identity!.Name;
            var user = _context.Users
                .Include(u => u.Lobbies)
                .FirstOrDefault(u => u.Username == username);     

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

        public async Task<ServiceResponse<GetGameDto>> CreateGame(CreateGameDto newGame)
        {
            var response = new ServiceResponse<GetGameDto>();
            if (await GameExists(newGame.Name))
            {
                response.Success = false;
                response.Message = "Game already exists";
                return response;
            }

            _encrypter.CreatePasswordHash(newGame.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var game = _mapper.Map<Game>(newGame);
            game.PasswordHash = passwordHash;
            game.PasswordSalt = passwordSalt;

            var user = GetCurrentUser();            

            var adminRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "GameAdmin");
            var lobbyRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "GameUser");
            if (adminRole == null || lobbyRole == null) 
            {
                response.Success = false;
                response.Message = "Role not found";
                return response;
            }

            var userAdminRole = new UserRole { User = user, Role = adminRole };
            var userLobbyRole = new UserRole { User = user, Role = lobbyRole };
            user.UserRoles.Add(userAdminRole);
            user.UserRoles.Add(userLobbyRole);

            game.Lobby.Users.Add(user);
            user.Lobbies.Add(game.Lobby);

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }

        public async Task<bool> GameExists(string gameName)
        {
            if (await _context.Games.AnyAsync(g => g.Name.ToLower() == gameName.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<GetGameDto>> JoinGameLobby(JoinGameDto game)
        {
            var response = new ServiceResponse<GetGameDto>();

            var gameToJoin = await _context.Games
                .Include(g => g.Lobby)
                .ThenInclude(l => l.Users)
                .FirstOrDefaultAsync(g => g.Name == game.Name);

            if (gameToJoin == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            if (!_encrypter.VerifyPasswordHash(game.Password, gameToJoin.PasswordHash, gameToJoin.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password";
                return response;
            }

            var user = GetCurrentUser();
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if(gameToJoin.Lobby.Users.Any(u => u.Username == user.Username))
            {
                response.Success = false;
                response.Message = "User is already in the lobby";
                return response;
            }

            gameToJoin.Lobby.Users.Add(user);

            user.Lobbies.Add(gameToJoin.Lobby);

            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(gameToJoin);

            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> DeleteGame(DeleteGameDto game)
        {
            var response = new ServiceResponse<GetGameDto>();
            var gameToDelete = await _context.Games
                .Include(g => g.Players)
                .Include(g => g.Lobby).ThenInclude(l => l.Users).ThenInclude(u => u.UserRoles)
                .FirstOrDefaultAsync(g => g.Name == game.Name);

            if(gameToDelete == null) {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            if (!_encrypter.VerifyPasswordHash(game.Password, gameToDelete.PasswordHash, gameToDelete.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password";
                return response;
            }

            foreach (var user in gameToDelete.Lobby.Users)
            {
                if (user.Lobbies.Contains(gameToDelete.Lobby))
                {
                    user.Lobbies.Remove(gameToDelete.Lobby);
                    user.UserRoles.Clear();
                }
            }                      

            _context.RemoveRange(gameToDelete.Players);

            _context.Remove(gameToDelete);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(gameToDelete);
            return response;
        }

        public async Task<ServiceResponse<List<GetGameDto>>> GetAllGames()
        {
            var response = new ServiceResponse<List<GetGameDto>>();
            var games = await _context.Games
                .Select(g => new GetGameDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    MaxPlayers = g.MaxPlayers,
                    ActivePlayers = g.ActivePlayers,
                    LobbyId = g.Lobby.Id
                })
                .ToListAsync();

            response.Data = _mapper.Map<List<GetGameDto>>(games);
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> BuyIn(int gameId)
        {
            var response = new ServiceResponse<GetGameDto>();
            var user = GetCurrentUser();
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                response.Success= false;
                response.Message = "Game not found";
                return response;
            }

            if(game!.Players.Any(p => p.UserId == user.Id))
            {
                response.Success = false;
                response.Message = "User is already a player in the game.";
                return response;
            }

            if(game.ActivePlayers >= game.MaxPlayers)
            {
                response.Success = false;
                response.Message = "Game is full.";
                return response;
            }

            if (!CanJoin(game))
            {
                response.Success = false;
                response.Message = "Game is full";
                return response;
            }

            var player = new Player
            {
                UserId = user.Id,
                GameId = game.Id,
                Chips = 1000
            };

            game.Players.Add(player);
            game.ActivePlayers++;
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
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
            var player = game!.Players.FirstOrDefault(p => p.UserId == user.Id);

            game.Players.Remove(player);
            game.ActivePlayers--;
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Player was removed from the table";
            response.Data = _mapper.Map<GetGameDto>(game);
            return response;
        }
    }
}
