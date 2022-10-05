using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.EmailServices;
using PrivateEye.Entites;
using PrivateEye.Identity;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IMailServices _mailService;
        private readonly IAdministartorRepository _administratorRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public AdministratorService(IMailServices mailService, IAdministartorRepository administratorRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _mailService = mailService;
            _administratorRepository = administratorRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> DeleteAdminAsync(int id)
        {
            var admin = await _administratorRepository.GetAsync(adminn => adminn.IsDeleted == false && adminn.Id == id);
            if (admin == null )
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }

            admin.IsDeleted = true;
            await _administratorRepository.UpdateAsync(admin);
            return new BaseResponse
            {
                Message = "Administrator Successfully Deleted",
                Success = true
            };
        }

        public async Task<AdministratorResponseModel> GetAdminAsync(int id)
        {
            var admin = await _administratorRepository.GetAdminByIdAsync(id);
            if (admin == null)
            {
                return new AdministratorResponseModel
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            return new AdministratorResponseModel
            {
                Data = new AdministratorDTO
                {
                    Id = admin.Id,
                    UserName = admin.User.UserName,
                    FirstName = admin.User.FirstName,
                    LastName = admin.User.LastName,
                    Password = admin.User.Password,
                    Email = admin.User.Email,
                    PhoneNumber = admin.User.PhoneNumber
                },
                Message = "Admin Successfully Retrieved",
                Success = true,

            };
        }

        public async Task<AdministratorsResponseModel> GetAllAdminAsync()
        {
                var admin = await _administratorRepository.GetAdministratorsAsync();
                if (admin == null)
                {
                    return new AdministratorsResponseModel
                    {
                        Message = "Admins List Empty",
                        Success = false
                    };
                }
                return new AdministratorsResponseModel
                {
                    Success = true,
                    Message = "Administrators Successfully Retrieved",
                    Data = admin.Select(administrator => new AdministratorDTO
                    {
                        Id = administrator.Id,
                        UserName = administrator.User.UserName,
                        FirstName = administrator.User.FirstName,
                        LastName = administrator.User.LastName,
                        Password = administrator.User.Password,
                        Email = administrator.User.Email,
                        PhoneNumber = administrator.User.PhoneNumber
                    }).ToList(),
                };
            
        }

        public async Task<BaseResponse> RegisterAdminAsync(AdministratorRequestModel model)
        {
            var admin = await _administratorRepository.GetAsync(admin => admin.User.Email == model.Email);
            if (admin != null)
            {
                return new BaseResponse
                {
                    Message = "Admin already exist",
                    Success = false
                };
            }

            var user = new User
            {
                Email = model.Email,
                UserName = $"{model.FirstName} {model.LastName}",
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                IsDeleted = false,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                

            };
            var role = await _roleRepository.GetAsync(rol => rol.Name == "Admin");
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Unable Find Administrator Role",
                    Success = false
                };
            }
            var userRole = new UserRole
            {
                User = user,
                UserId = user.Id,
                Role = role,
                RoleId = role.Id
            };
            var admins = new Administrator
            {
                User = user,
                UserId = user.Id,
                IsDeleted = false
            };

            await _userRepository.CreateAsync(user);
            user.UserRoles.Add(userRole);
            var adminss = await _administratorRepository.CreateAsync(admins);

            var mailRequest = new MailRequest
            {
                Subject = "Welcome To Private Eye",
                ToEmail = user.Email,
                ToName = user.FirstName + " " + user.LastName,
                HtmlContent = $"<html><body><h1>Hello {user.FirstName + " " + user.LastName}, Welcome to Private Eye Security Software. A new dawn in the Privacy World</h1></body></html>",
            };
            _mailService.SendEMailAsync(mailRequest);
            return new BaseResponse
            {
                Message = "Admin created successfully",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateAdminAsync(int id, UpdateAdministratorRequestModel model)
        {
            var admin = await _administratorRepository.GetAdminByIdAsync(id);
            if (admin == null )
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            admin.User.Password = model.Password;
            admin.User.UserName = model.UserName;
            admin.User.PhoneNumber = model.PhoneNumber;
            admin.User.Email = model.Email;
            
            await _administratorRepository.UpdateAsync(admin);
            return new BaseResponse
            {
                Message = "Admin Successfully Updated",
                Success = true
            };
        }
    }
    
}
