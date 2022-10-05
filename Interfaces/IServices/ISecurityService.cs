using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface ISecurityService
    {
        Task<BaseResponse> RegisterSecurityAsync(SecurityRequestModel model);

        Task<BaseResponse> UpdateSecurityAsync(int id, UpdateSecurityRequestModel model);

        Task<SecurityResponseModel> GetSecurityAsync(int id);

        Task<SecuritiesResponseModel> GetAllSecuritiesAsync();

        Task<BaseResponse> DeleteSecurityAsync(int id);
    }
}
