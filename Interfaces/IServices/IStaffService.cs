using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface IStaffService
    {
        Task<BaseResponse> RegisterStaffAsync(StaffRequestModel model);

        Task<BaseResponse> UpdateStaffAsync(int id, UpdateStaffRequestModel model);

        Task<StaffResponseModel> GetStaffAsync(int id);

        Task<StaffsResponseModel> GetAllStaffsAsync();

        Task<BaseResponse> DeleteStaffAsync(int id);

        Task<UserImagesResponseModel> GetAllStaffImagesAsync();
    }
}
