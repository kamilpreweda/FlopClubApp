namespace FlopClub.Services.TableService
{
    public interface ITableService
    {
        Task<ServiceResponse<GetGameDto>> BuyIn(int gameId);
        Task<ServiceResponse<GetGameDto>> LeaveTable(int gameId);
        Task<ServiceResponse<GetGameDto>> SetPositions(int gameId);
    }
}
