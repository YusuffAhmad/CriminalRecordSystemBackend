using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly ICameraService _cameraService;

        public CameraController(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CameraRequestModel request)
        {
            var isSuccessful = await _cameraService.RegisterCameraAsync(request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateCameraRequestModel request)
        {
            var isSuccessful = await _cameraService.UpdateCameraAsync(id, request);
            if (isSuccessful.Success == false)
            {
                return BadRequest(isSuccessful);
            }
            return Ok(isSuccessful);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllCamerasAsync()
        {
            var Camera = await _cameraService.GetAllCameraAsync();
            if (Camera == null)
            {
                return BadRequest(Camera);
            }
            return Ok(Camera.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetCameraAsync([FromRoute] int id)
        {
            var Camera = await _cameraService.GetCameraAsync(id);
            if (Camera == null)
            {
                return BadRequest(Camera);
            }
            return Ok(Camera.Data);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var Camera = await _cameraService.DeleteCameraAsync(id);
            if (Camera == null)
            {
                return BadRequest(Camera);
            }
            return Ok(Camera);
        }
    }
}
