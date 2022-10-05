using PrivateEye.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class UserResponseModel : BaseResponse
    {
        public UserDTO Data { get; set; } 
    }
    
    public class UsersResponseModel : BaseResponse
    {
        public IEnumerable<UserDTO> Data { get; set; }
    }
    
    public class UserLoginResponseModel : BaseResponse
    {
        public string Token { get; set; }
        public UserDTO Data { get; set; }
    }
}
