using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs.ResponseModels
{
    public class UserImagesResponseModel : BaseResponse
    {
        public ICollection<string> Data { get; set; }
    }
}
