namespace FlopClub.Services.RoleService
{
    public interface IRoleService
    {
        Task<ServiceResponse<GetRoleDto>> CreateRole(CreateRoleDto newRole);
        Task<ServiceResponse<List<GetRoleDto>>> GetAllRoles();
        Task<GetRoleDto> GetRoleByName(string roleName);
        Task<ServiceResponse<GetUserDto>> AssignRoleToUser(int userId, int roleId);
        Task<ServiceResponse<GetUserDto>> RemoveRoleFromUser(int userId, int roleId);
        Task<ServiceResponse<bool>> DeleteRole(int roleId);
    }
}
