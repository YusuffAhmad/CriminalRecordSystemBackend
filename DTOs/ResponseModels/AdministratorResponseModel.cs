using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class AdministratorResponseModel : BaseResponse
    {
        public AdministratorDTO Data { get; set; }
    }
    
    public class AdministratorsResponseModel : BaseResponse
    {
        public List<AdministratorDTO> Data { get; set; }
    }
}
