using Microsoft.AspNetCore.Http;
using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IServices
{
    public interface IUserService
    {
        Task<UserResponseModel> LoginAsync(UserLoginRequestModelModel model);
        Task<UsersResponseModel> GetAllUserAsync();
        Task<UserResponseModel> GetUserAsync(int id);
        Task<BaseResponse> DeleteUserAsync(int id);
        Task<BaseResponse> VerifyAsync(string imageUrl);
        Task<string> GetFaceIDAsync(string imageUrl);
        string  SaveDataUrlAsImage(string dataUrl);
    }
}
