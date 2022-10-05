using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class StaffResponseModel : BaseResponse
    {
        public StaffDTO Data { get; set; }
    }
    
    public class StaffsResponseModel : BaseResponse
    {
        public IEnumerable<StaffDTO> Data { get; set; }
    }
}
