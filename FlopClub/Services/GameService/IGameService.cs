namespace FlopClub.Services.GameService
{
    public interface IGameService
    {
        Task<ServiceResponse<GetGameDto>> CreateGame(CreateGameDto newGame);
        Task<bool> GameExists(string gameName);
        Task<ServiceResponse<GetGameDto>> JoinGameLobby(JoinGameDto game);
        Task<ServiceResponse<string>> LeaveGameLobby(int gameId);
        Task<ServiceResponse<GetGameDto>> DeleteGame(DeleteGameDto game);
        Task<ServiceResponse<List<GetGameDto>>> GetAllGames();
        Task<ServiceResponse<GetGameDto>> BuyIn(int gameId);
        Task<ServiceResponse<GetGameDto>> LeaveTable(int gameId);
    }
}
