using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class CameraResponseModel : BaseResponse
    {
        public CameraDTO Data { get; set; }
    }
    
    public class CamerasResponseModel : BaseResponse
    {
        public IEnumerable<CameraDTO> Data { get; set; }
    }
}
