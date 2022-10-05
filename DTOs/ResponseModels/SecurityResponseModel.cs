using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class SecurityResponseModel : BaseResponse
    {
        public SecurityDTO Data { get; set; }
    }
    
    public class SecuritiesResponseModel : BaseResponse
    {
        public IEnumerable<SecurityDTO> Data { get; set; }
    }
}
