using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class RoleResponseModel : BaseResponse
    {
       public RoleDTO Data { get; set; } 
    }
    
    public class RolesResponseModel : BaseResponse
    {
        public IEnumerable<RoleDTO> Data { get; set; }
    }
}
