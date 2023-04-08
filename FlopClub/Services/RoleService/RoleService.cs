namespace FlopClub.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RoleService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetRoleDto>> CreateRole(CreateRoleDto newRole)
        {
            var response = new ServiceResponse<GetRoleDto>();

            var role = new Role { Name = newRole.Name };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetRoleDto>(role);
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteRole(int id)
        {
            var response = new ServiceResponse<bool>();

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                response.Success = false;
                response.Message = "Role not found";
                return response;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            response.Data = true;
            return response;
        }

        public async Task<ServiceResponse<List<GetRoleDto>>> GetAllRoles()
        {
            var response = new ServiceResponse<List<GetRoleDto>>();

            var roles = await _context.Roles.ToListAsync();
            response.Data = _mapper.Map<List<GetRoleDto>>(roles);

            return response;
        }

        public async Task<GetRoleDto> GetRoleByName(string roleName)
        {
            var response = new ServiceResponse<GetRoleDto>();
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                response.Success = false;
                response.Message = "Role not found";
                return response.Data;
            }
            return _mapper.Map<GetRoleDto>(role);
        }

        public async Task<ServiceResponse<GetUserDto>> AssignRoleToUser(int userId, int roleId)
        {
            var response = new ServiceResponse<GetUserDto>();

            var user = await _context.Users.Include(u => u.UserRoles).SingleOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == roleId);

            if (user == null || role == null)
            {
                response.Success = false;
                response.Message = "User or role not found";
                return response;
            }

            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
            {
                response.Success = false;
                response.Message = "User already has this role";
                return response;
            }

            user.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetUserDto>(user);
            return response;
        }     

        public async Task<ServiceResponse<GetUserDto>> RemoveRoleFromUser(int userId, int roleId)
        {
            var response = new ServiceResponse<GetUserDto>();

            var user = await _context.Users.Include(u => u.UserRoles).SingleOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == roleId);

            if (user == null || role == null)
            {
                response.Success = false;
                response.Message = "User or role not found";
                return response;
            }

            var userRole = user.UserRoles.SingleOrDefault(ur => ur.RoleId == roleId);
            if (userRole == null)
            {
                response.Success = false;
                response.Message = "User does not have this role";
                return response;
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetUserDto>(user);
            return response;
        }
    }
}
