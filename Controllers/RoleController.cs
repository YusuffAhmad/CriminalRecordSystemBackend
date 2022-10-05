using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm]RoleRequestModel request)
        {
           var isSuccessful = await _roleService.RegisterRoleAsync(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]  int id, [FromBody] UpdateRoleRequestModel request)
        {
            var isSuccessful = await _roleService.UpdateRoleAsync(id, request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var Role = await _roleService.GetAllRoleAsync();
            if (Role == null)
            {
                return BadRequest(Role);
            }
            return Ok(Role.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetRoleAsync([FromRoute] int id)
        {
            var Role = await _roleService.GetRoleAsync(id);
            if (Role == null)
            {
                return BadRequest(Role);
            }
            return Ok(Role.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var Role = await _roleService.DeleteRoleAsync(id);
            if (Role == null)
            {
                return BadRequest(Role);
            }
            return Ok(Role);
        }
    }
}
