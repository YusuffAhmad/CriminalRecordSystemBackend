using PrivateEye.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Auth
{
    public interface IJWTAuthenticationManager
    {
        public string GenerateToken(UserDTO user);
    }
}
