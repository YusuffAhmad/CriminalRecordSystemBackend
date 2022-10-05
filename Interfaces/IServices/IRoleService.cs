using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface IRoleService
    { 
        Task<BaseResponse> RegisterRoleAsync(RoleRequestModel model);

        Task<BaseResponse> UpdateRoleAsync(int id, UpdateRoleRequestModel model);

        Task<RoleResponseModel> GetRoleAsync(int id);

        Task<RolesResponseModel> GetAllRoleAsync();

        Task<BaseResponse> DeleteRoleAsync(int id);
    }
}
