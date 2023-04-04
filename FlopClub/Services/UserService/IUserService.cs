namespace FlopClub.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> Delete(int id);
    }
}
