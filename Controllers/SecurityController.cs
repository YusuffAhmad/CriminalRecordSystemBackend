using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromForm]SecurityRequestModel request)
        {
            var isSuccessful = await _securityService.RegisterSecurityAsync(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateSecurityRequestModel request)
        {
            var isSuccessful = await _securityService.UpdateSecurityAsync(id, request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllSecuritysAsync()
        {
            var Security = await _securityService.GetAllSecuritiesAsync();
            if (Security == null)
            {
                return BadRequest(Security);
            }
            return Ok(Security.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetSecurityAsync([FromRoute] int id)
        {
            var Security = await _securityService.GetSecurityAsync(id);
            if (Security == null)
            {
                return BadRequest(Security);
            }
            return Ok(Security.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var Security = await _securityService.DeleteSecurityAsync(id);
            if (Security == null)
            {
                return BadRequest(Security);
            }
            return Ok(Security);
        }
    }
}
