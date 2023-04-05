namespace FlopClub.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> Delete(int id);

        Task<ServiceResponse<List<GetUserDto>>> GetAll();

        Task<ServiceResponse<GetUserDto>> Get(int id);
    }
}
