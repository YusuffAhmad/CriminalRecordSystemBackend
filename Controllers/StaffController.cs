using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm]StaffRequestModel request)
        {
            var isSuccessful = await _staffService.RegisterStaffAsync(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateStaffRequestModel request)
        {
            var isSuccessful = await _staffService.UpdateStaffAsync(id, request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllStaffsAsync()
        {
            var Staff = await _staffService.GetAllStaffsAsync();
            if (Staff == null)
            {
                return BadRequest(Staff);
            }
            return Ok(Staff.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetStaffAsync([FromRoute] int id)
        {
            var Staff = await _staffService.GetStaffAsync(id);
            if (Staff == null)
            {
                return BadRequest(Staff);
            }
            return Ok(Staff.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var Staff = await _staffService.DeleteStaffAsync(id);
            if (Staff == null)
            {
                return BadRequest(Staff);
            }
            return Ok(Staff);
        }
    }
}
