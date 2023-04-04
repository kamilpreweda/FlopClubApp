using FlopClub.Models;

namespace FlopClub.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetUserDto>> Delete(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToDelete == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            _context.Remove(userToDelete);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetUserDto>(userToDelete);
            return response;
        }
    }
}
