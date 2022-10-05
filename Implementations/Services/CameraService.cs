using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.Entites;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Services
{
    public class CameraService : ICameraService
    {
        private readonly ICameraRepository _cameraRepository;

        public CameraService(ICameraRepository cameraRepository)
        {
            _cameraRepository = cameraRepository;
        }

        public async Task<BaseResponse> DeleteCameraAsync(int id)
        {
            var camera = await _cameraRepository.GetAsync(cameraInstance => cameraInstance.IsDeleted == false && cameraInstance.Id == id);
            if (camera == null)
            {
                return new BaseResponse
                {
                    Message = "Camera not found",
                    Success = false
                };
            }

            camera.IsDeleted = true;
            await _cameraRepository.UpdateAsync(camera);
            return new BaseResponse
            {
                Message = "Camera Successfully Deleted",
                Success = true
            };
        }

        public async Task<CameraResponseModel> GetCameraAsync(int id)
        {
            var camera = await _cameraRepository.GetAsync(CameraInstance => CameraInstance.IsDeleted == false && CameraInstance.Id == id);
            if (camera == null)
            {
                return new CameraResponseModel
                {
                    Message = "Camera not found",
                    Success = false
                };
            }
            return new CameraResponseModel
            {
                Data = new CameraDTO
                {
                    Id = camera.Id,
                    IPAddress = camera.IPAddress,
                    MacAddress = camera.MacAddress,
                    UID = camera.UID,
                    DeviceType = camera.DeviceType,
                    RegistrationMode = camera.RegistrationMode,
                    Name = camera.Name
                },
                Message = "Camera Successfully Retrieved",
                Success = true,

            };
        }

        public async Task<CamerasResponseModel> GetAllCameraAsync()
        {
            var cammera = await _cameraRepository.GetAllAsync();
            if (cammera == null)
            {
                return new CamerasResponseModel
                {
                    Message = "Camera List Empty",
                    Success = false
                };
            }
            return new CamerasResponseModel
            {
                Success = true,
                Message = "Cameraistrators Successfully Retrieved",
                Data = cammera.Select(camera => new CameraDTO
                {
                    Id = camera.Id,
                    IPAddress = camera.IPAddress,
                    MacAddress = camera.MacAddress,
                    UID = camera.UID,
                    DeviceType = camera.DeviceType,
                    RegistrationMode = camera.RegistrationMode,
                    Name = camera.Name
                }).ToList(),
            };
        }

        public async Task<BaseResponse> RegisterCameraAsync(CameraRequestModel model)
        {
            var camera = await _cameraRepository.GetAsync(cameraInstance => cameraInstance.IPAddress == model.IPAddress);
            if (camera != null)
            {
                return new BaseResponse
                {
                    Message = "Camera already exist",
                    Success = false
                };
            }
            var newCamera = new Camera
            {
                IPAddress = model.IPAddress,
                RegistrationMode = model.RegistrationMode,
                DeviceType = model.DeviceType,
                UID = model.UID,
                MacAddress = model.MacAddress,
                Name = model.Name,
                IsDeleted = false
            };
           var isSuccess = await _cameraRepository.CreateAsync(newCamera);
            if (camera == null)
            {
                return new BaseResponse
                {
                    Message = "Camera created successfully",
                    Success = true
                };
            }
            return new BaseResponse
            {
                Message = "Unanble To Add The Camera",
                Success = false
            };
        }

        public async Task<BaseResponse> UpdateCameraAsync(int id, UpdateCameraRequestModel model)
        {
            var camera = await _cameraRepository.GetAsync(newCamera => newCamera.IsDeleted == false && newCamera.Id == id);
            if (camera == null)
            {
                return new BaseResponse
                {
                    Message = "Camera not found",
                    Success = false
                };
            }
            camera.IPAddress = model.IPAddress;
            camera.Name = model.Name;
            camera.UID = model.UID;
            camera.RegistrationMode = model.RegistrationMode;
            camera.MacAddress = model.MacAddress;
            camera.DeviceType = model.DeviceType;
                 
            await _cameraRepository.UpdateAsync(camera);
            return new BaseResponse
            {
                Message = "Camera Successfully Updated",
                Success = true
            };
        }
    }
}

