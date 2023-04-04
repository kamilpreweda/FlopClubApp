﻿using Microsoft.EntityFrameworkCore;

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
            var user = _context.Users.FirstOrDefault(u => u.Username == username);     

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
            if(await GameExists(newGame.Name))
            {
                response.Success = false;
                response.Message = "Game already exists";
                return response;
            }

            _encrypter.CreatePasswordHash(newGame.Password, out byte[] passwordHash, out byte[] passwordSalt);
   
            var game = _mapper.Map<Game>(newGame);
            game.PasswordHash = passwordHash;
            game.PasswordSalt = passwordSalt;

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
                .ThenInclude(g => g.Users)
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

            //  LATER CHECK THIS CONDITION
            //if (CanJoin(game))
            //{
            //    response.Success = false;
            //    response.Message = "Game is full";
            //    return response;
            //}

            var user = GetCurrentUser();
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            // ADD IN BUY IN / JOIN THE TABLE
            //if (game.Players.Any(p => p.User!.Id == user.Id))
            //{
            //    response.Success = false;
            //    response.Message = "User already joined the game";
            //    return response;
            //}

            if(gameToJoin.Lobby.Users.Any(u => u.Username == user.Username))
            {
                response.Success = false;
                response.Message = "User is already in the lobby";
                return response;
            }

            // ADD IN BUY IN / JOIN THE TABLE
            //var player = new Player
            //{
            //    User = user
            //};

            gameToJoin.Lobby.Users.Add(user);
            user.LobbiesIds.Add(gameToJoin.Lobby.Id);

            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(gameToJoin);

            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> DeleteGame(DeleteGameDto game)
        {
            var response = new ServiceResponse<GetGameDto>();
            var gameToDelete = await _context.Games
                .Include(g => g.Players)
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

            foreach(var user in gameToDelete.Lobby.Users)
            {
                if(user.LobbiesIds.Contains(gameToDelete.Lobby.Id))
                {
                    user.LobbiesIds.Remove(gameToDelete.Lobby.Id);
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
                .Include(g => g.Lobby.Users)
                .ToListAsync();

            response.Data = _mapper.Map<List<GetGameDto>>(games);
            return response;
        }
    }
}
