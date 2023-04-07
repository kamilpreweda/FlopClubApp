using FlopClub.Services.RoleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlopClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRoleDto>>> GetAll()
        {
            var serviceResponse = await _roleService.GetAllRoles();
            if (serviceResponse.Success)
            {
                var roles = serviceResponse.Data;
                return Ok(roles);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<GetRoleDto>> Create(CreateRoleDto newRole)
        {
            var serviceResponse = await _roleService.CreateRole(newRole);
            if (serviceResponse.Success)
            {
                var role = serviceResponse.Data;
                return Ok(role);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var serviceResponse = await _roleService.DeleteRole(id);
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse.Data);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> AssignRoleToUser(AssignRoleDto assignRole)
        {
            var response = await _roleService.AssignRoleToUser(assignRole.UserId, assignRole.RoleId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("remove")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> RemoveRoleFromUser(RemoveRoleDto removeRole)
        {
            var response = await _roleService.RemoveRoleFromUser(removeRole.UserId, removeRole.RoleId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}