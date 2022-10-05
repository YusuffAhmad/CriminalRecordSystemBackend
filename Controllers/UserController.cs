using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateEye.Auth;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;

        public UserController(IUserService userService, IJWTAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LogInAsync([FromBody]UserLoginRequestModelModel model)
        {
            var user = await _userService.LoginAsync(model);
            if (user.Data != null)
            {
                var token = _jwtAuthenticationManager.GenerateToken(user.Data);
                var response = new UserLoginResponseModel
                {
                    Data = user.Data,
                    Message = user.Message,
                    Success = user.Success,
                    Token = token,
                };
                return Ok(response);
            }
            return BadRequest(user);
        }
        
        [HttpPost("CompareFaces")]
        public async Task<IActionResult> CompareFacesAsync([FromBody]string dataUrl)
        {
            var response = await _userService.VerifyAsync(dataUrl);
            //var response = await _userService.CompareFaces(dataUrl);
            if (response.Success != false)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var user = await _userService.GetAllUserAsync();
            if (user == null) 
            {
                return BadRequest(user);
            }
            return Ok(user.Data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserAsyncAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user.Data);
        }

        
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]  int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
    }
}
