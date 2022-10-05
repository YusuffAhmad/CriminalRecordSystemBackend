using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.Identity;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleManager)
        {
            _roleRepository = roleManager;
        }

        public async Task<BaseResponse> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Role not found",
                    Success = false
                };
            }

            role.IsDeleted = true;
            await _roleRepository.UpdateAsync(role);
            return new BaseResponse
            {
                Message = "Role Successfully Updated",
                Success = true
            };
        }

        public async Task<RolesResponseModel> GetAllRoleAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            if (roles == null)
            {
                return new RolesResponseModel
                {
                    Message = "Roles not found",
                    Success = false
                };
            }
            return new RolesResponseModel
            {
                Success = true,
                Message = "Roles Successfully Retrieved",
                Data = roles.Select( role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                }).ToList(),
            };
        }

        public async Task<RoleResponseModel> GetRoleAsync(int id)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (role == null )
            {
                return new RoleResponseModel
                {
                    Message = "Role not found",
                    Success = false
                };
            }
            return new RoleResponseModel
            {
                Success = true,
                Message = "Role Successfully Retrieved",
                Data = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                },
            };
               
        }

        public async Task<BaseResponse> RegisterRoleAsync(RoleRequestModel model)
        {
            var role = await _roleRepository.GetAsync(x => x.Name == model.Name && x.IsDeleted == false);
            if (role != null)
            {
                return new BaseResponse
                {
                    Message = "Role Already Exist",
                    Success = false
                };
            }
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description,
                
            };
            var isSuccessful = await _roleRepository.CreateAsync(newRole);
            if (isSuccessful != null)
            {
                return new BaseResponse
                {
                    Message = "Role Successfully Created",
                    Success = true
                };
            }
            return new BaseResponse
            {
                Message = "Unable to Add Role",
                Success = false
            };

        }

        public async Task<BaseResponse> UpdateRoleAsync(int id, UpdateRoleRequestModel model)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Role not found",
                    Success = false
                };
            }
            
            role.Name = model.Name;
            role.Description = model.Description;
            await _roleRepository.UpdateAsync(role);
            return new BaseResponse
            {
                Message = "Role Successfully Updated",
                Success = true
            };
        }
    }
}
