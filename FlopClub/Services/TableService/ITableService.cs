namespace FlopClub.Services.TableService
{
    public interface ITableService
    {
        Task<ServiceResponse<GetGameDto>> BuyIn(int gameId);
        Task<ServiceResponse<GetGameDto>> LeaveTable(int gameId);
        Task<ServiceResponse<GetGameDto>> SetPositions(int gameId);
        Task<ServiceResponse<GetGameDto>> StartGame(int gameId);
        Task<ServiceResponse<GetGameDto>> PauseGame(int gameId);
    }
}
