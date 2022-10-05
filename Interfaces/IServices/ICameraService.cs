using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface ICameraService
    {
        Task<BaseResponse> RegisterCameraAsync(CameraRequestModel model);

        Task<BaseResponse> UpdateCameraAsync(int id, UpdateCameraRequestModel model);

        Task<CameraResponseModel> GetCameraAsync(int id);

        Task<CamerasResponseModel> GetAllCameraAsync();

        Task<BaseResponse> DeleteCameraAsync(int id);
    }
}
