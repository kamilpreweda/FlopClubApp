﻿using FlopClub.Services.RoleService;
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
        private readonly IRoleService _roleService;

        public GameService(DataContext context, IEncrypter encrypter, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRoleService roleService)
        {
            _context = context;
            _encrypter = encrypter;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }

        private User GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext!.User!.Identity!.Name;
            var user = _context.Users
                .Include(u => u.Lobbies)
                .SingleOrDefault(u => u.Username == username);     

            return user!;
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

            var adminRole = await _roleService.GetRoleByName("GameAdmin");
            var lobbyRole = await _roleService.GetRoleByName("GameUser");

            if (adminRole == null || lobbyRole == null) 
            {
                response.Success = false;
                response.Message = "Role not found";
                return response;
            }

            await _roleService.AssignRoleToUser(user.Id, adminRole.Id);
            await _roleService.AssignRoleToUser(user.Id, lobbyRole.Id);

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
                .SingleOrDefaultAsync(g => g.Name == game.Name);

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

            var lobbyRole = await _roleService.GetRoleByName("GameUser");
            await _roleService.AssignRoleToUser(user.Id, lobbyRole.Id);

            gameToJoin.Lobby.Users.Add(user);

            user.Lobbies.Add(gameToJoin.Lobby);

            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetGameDto>(gameToJoin);

            return response;
        }

        public async Task<ServiceResponse<string>> LeaveGameLobby(int gameId)
        {
            var response = new ServiceResponse<string>();
            var user = GetCurrentUser();
            var game = await _context.Games
                .Include(g => g.Lobby)
                .ThenInclude(l => l.Users).ThenInclude(u => u.UserRoles)
                .SingleOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            var lobby = game.Lobby;
            var lobbyRole = await _roleService.GetRoleByName("GameUser");
            var adminRole = await _roleService.GetRoleByName("GameAdmin");

            if (lobbyRole == null || adminRole == null)
            {
                response.Success = false;
                response.Message = "Role not found";
                return response;
            }

            if (user.UserRoles!.Any(ur => ur.Role.Name == "GameAdmin"))
            {
                if (lobby.Users.Count == 1)
                {
                    // Delete game if user is the only one left in the lobby
                    await DeleteGame(_mapper.Map<DeleteGameDto>(game));
                    response.Data = "Game deleted";
                    return response;
                }
                else
                {
                    // Randomly assign the GameAdmin role to another user in the lobby
                    var nextAdmin = lobby.Users.Except(new[] { user })
                        .OrderBy(x => Guid.NewGuid())
                        .FirstOrDefault();

                    if (nextAdmin == null)
                    {
                        response.Success = false;
                        response.Message = "Failed to assign GameAdmin role";
                        return response;
                    }

                    await _roleService.RemoveRoleFromUser(user.Id, adminRole.Id);
                    await _roleService.AssignRoleToUser(nextAdmin.Id, adminRole.Id);
                }
            }

            lobby.Users.Remove(user);
            user.Lobbies.Remove(lobby);

            await _roleService.RemoveRoleFromUser(user.Id, lobbyRole.Id);
            await _context.SaveChangesAsync();

            response.Data = "User successfully removed from game lobby";
            return response;
        }

        public async Task<ServiceResponse<GetGameDto>> DeleteGame(DeleteGameDto game)
        {
            var response = new ServiceResponse<GetGameDto>();
            var gameToDelete = await _context.Games
                .Include(g => g.Players)
                .Include(g => g.Lobby).ThenInclude(l => l.Users).ThenInclude(u => u.UserRoles!).ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(g => g.Name == game.Name);

            if(gameToDelete == null) {
                response.Success = false;
                response.Message = "Game not found";
                return response;
            }

            var currentUser = GetCurrentUser();
            var isAdmin = currentUser.UserRoles!.Any(ur => ur.Role.Name == "GameAdmin");

            if (!isAdmin && !_encrypter.VerifyPasswordHash(game.Password, gameToDelete.PasswordHash, gameToDelete.PasswordSalt))
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
                    user.UserRoles!.Clear();
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

        
    }
}
