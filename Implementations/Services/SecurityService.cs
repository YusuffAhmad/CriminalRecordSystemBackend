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
    public class SecurityService : ISecurityService
    {
        private readonly IMailServices _mailService;
        private readonly ISecurityRepository _securityRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public SecurityService(IMailServices mailService, ISecurityRepository securityRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _mailService = mailService;
            _securityRepository = securityRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> DeleteSecurityAsync(int id)
        {
            var security = await _securityRepository.GetAsync(securityInstance => securityInstance.IsDeleted == false && securityInstance.Id == id);
            if (security == null)
            {
                return new BaseResponse
                {
                    Message = "Security not found",
                    Success = false
                };
            }

            security.IsDeleted = true;
            await _securityRepository.UpdateAsync(security);
            return new BaseResponse
            {
                Message = "Security Successfully Deleted",
                Success = true
            };
        }

        public async Task<SecuritiesResponseModel> GetAllSecuritiesAsync()
        {
            var securities = await _securityRepository.GetAllSecuritiesAsync();
            if (securities == null)
            {
                return new SecuritiesResponseModel
                {
                    Message = "Securities List is Empty",
                    Success = false
                };
            }
            return new SecuritiesResponseModel
            {
                Success = true,
                Message = "Securities Successfully Retrieved",
                Data = securities.Select(security => new SecurityDTO
                {
                    Id = security.Id,
                    Email = security.User.Email,
                    PhoneNumber = security.User.PhoneNumber,
                    AgencyName = security.AgencyName,
                    Password = security.User.Password,
                }).ToList(),
            };
        }

        public async Task<SecurityResponseModel> GetSecurityAsync(int id)
        {
            var security = await _securityRepository.GetSecurityByIdAsync(id);
            if (security == null)
            {
                return new SecurityResponseModel
                {
                    Message = "Security not found",
                    Success = false
                };
            }
            return new SecurityResponseModel
            {
                Data = new SecurityDTO
                {
                    Id = security.Id,
                    Email = security.User.Email,
                    PhoneNumber = security.User.PhoneNumber,
                    AgencyName = security.AgencyName,
                    Password = security.User.Password,
                },
                Message = "Security Successfully Retrieved",
                Success = true,

            };
        }

        public async Task<BaseResponse> RegisterSecurityAsync(SecurityRequestModel model)
        {
            var security = await _securityRepository.GetAsync(admin => admin.User.Email == model.Email);
            if (security != null)
            {
                return new BaseResponse
                {
                    Message = "Security already exist",
                    Success = false
                };
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.AgencyName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                IsDeleted = false,
                LastName = model.AgencyName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.AgencyName
            };
            var role = await _roleRepository.GetAsync(use => use.Name == "Security");
            if (role == null )
            {
                return new BaseResponse
                {
                    Message = "Unable To Add Security",
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

            var newSecurity = new Security
            {
                User = user,
                UserId = user.Id,
                AgencyName = model.AgencyName,
                IsDeleted = false,

            };
            await _userRepository.CreateAsync(user);
            user.UserRoles.Add(userRole);
            var sec = await _securityRepository.CreateAsync(newSecurity);
            var mailRequest = new MailRequest
            {
                Subject = "Welcome To Private Eye",
                ToEmail = user.Email,
                ToName = user.FirstName + user.LastName,
                HtmlContent = $"<html><body><h1>Hello {user.FirstName + user.LastName}, Welcome to Private Eye Security Software. A new dawn in the Privacy World</h1></body></html>",
            };
            _mailService.SendEMailAsync(mailRequest);
            return new BaseResponse
            {
                Message = "Security created successfully",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateSecurityAsync(int id, UpdateSecurityRequestModel model)
        {
            var security = await _securityRepository.GetSecurityByIdAsync(id);
            if (security == null)
            {
                return new BaseResponse
                {
                    Message = "Staff not found",
                    Success = false
                };
            }
            security.User.Password = model.Password;
            security.User.UserName = model.AgencyName;
            security.User.PhoneNumber = model.PhoneNumber;
            security.User.Email = model.Email;
            await _securityRepository.UpdateAsync(security);
            return new BaseResponse
            {
                Message = "Staff Successfully Updated",
                Success = true
            };
        }
    }
}

