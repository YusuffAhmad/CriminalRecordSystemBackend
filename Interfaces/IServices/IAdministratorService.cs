using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface IAdministratorService
    {
        Task<BaseResponse> RegisterAdminAsync(AdministratorRequestModel model);

        Task<BaseResponse> UpdateAdminAsync(int id, UpdateAdministratorRequestModel model);

        Task<AdministratorResponseModel> GetAdminAsync(int id);

        Task<AdministratorsResponseModel> GetAllAdminAsync();

        Task<BaseResponse> DeleteAdminAsync(int id);
    }
}
