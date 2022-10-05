using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class AdministratorsController : ControllerBase
    {
        private readonly IAdministratorService _adminService;

        public AdministratorsController(IAdministratorService administaratorService)
        {
            _adminService = administaratorService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm]AdministratorRequestModel request)
        {
            var isSuccessful = await _adminService.RegisterAdminAsync(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateAdministratorRequestModel request)
        {
            var isSuccessful = await _adminService.UpdateAdminAsync(id, request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllAdminsAsync()
        {
            var Admin = await _adminService.GetAllAdminAsync();
            if (Admin == null)
            {
                return BadRequest(Admin);
            }
            return Ok(Admin.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetAdminAsync([FromRoute] int id)
        {
            var Admin = await _adminService.GetAdminAsync(id);
            if (Admin == null)
            {
                return BadRequest(Admin);
            }
            return Ok(Admin.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var Admin = await _adminService.DeleteAdminAsync(id);
            if (Admin == null)
            {
                return BadRequest(Admin);
            }
            return Ok();
        }
    }
}
