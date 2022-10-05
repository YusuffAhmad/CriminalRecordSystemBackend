using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.EmailServices;
using PrivateEye.Entites;
using PrivateEye.Identity;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Services
{
    public class StaffService : IStaffService
    {
        private readonly IMailServices _mailService;
        private readonly IStaffRepository _staffRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public StaffService(IMailServices mailService, IStaffRepository staffRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _mailService = mailService;
            _staffRepository = staffRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> DeleteStaffAsync(int id)
        {
            var staff = await _staffRepository.GetAsync(satffInstance => satffInstance.IsDeleted == false && satffInstance.Id == id);
            if (staff == null)
            {
                return new BaseResponse
                {
                    Message = "Staff not found",
                    Success = false
                };
            }

            staff.IsDeleted = true;
            await _staffRepository.UpdateAsync(staff);
            return new BaseResponse
            {
                Message = "Staff Successfully Deleted",
                Success = true
            };
        }

        public async Task<StaffsResponseModel> GetAllStaffsAsync()
        {
            var staffs = await  _staffRepository.GetAllStaffsAsync();
            if (staffs == null)
            {
                return new  StaffsResponseModel
                {
                    Message = "Staff List is Empty",
                    Success = false
                };
            }
            return new StaffsResponseModel
            {
                Success = true,
                Message = "Staffs Successfully Retrieved",
                Data = staffs.Select(staff => new StaffDTO
                {
                    Id = staff.Id,
                    Email = staff.User.Email,
                    ProfilePictureUrl = staff.ProfilePictureUrl,
                    PhoneNumber = staff.User.PhoneNumber,
                    Password = staff.User.Password,
                    FirstName = staff.User.FirstName,
                    LastName = staff.User.LastName,
                    UserName = staff.User.UserName 
                }).ToList(),
            };
        }

        public async Task<StaffResponseModel> GetStaffAsync(int id)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return new StaffResponseModel
                {
                    Message = "Staff not found",
                    Success = false
                };
            }
            return new StaffResponseModel
            {
                Data = new StaffDTO
                {
                    Id = staff.Id,
                    Email = staff.User.Email,
                    ProfilePictureUrl = staff.ProfilePictureUrl,
                    PhoneNumber = staff.User.PhoneNumber,
                    Password = staff.User.Password,
                    FirstName = staff.User.FirstName,
                    LastName = staff.User.LastName,
                    UserName = staff.User.UserName
                },
                Message = "Staff Successfully Retrieved",
                Success = true,

            };
        }

        public async Task<BaseResponse> RegisterStaffAsync(StaffRequestModel model)
        {
            var staff = await _staffRepository.GetAsync(admin => admin.User.Email == model.Email && admin.IsDeleted == false);
            if (staff != null)
            {
                return new BaseResponse
                {
                    Message = "Staff already exist",
                    Success = false
                };
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword( model.Password),
                IsDeleted = false,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName
            };
            string fileeName = null;
            if (model.ProfilePictureUrl != null)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\ProfilePictures\\");
                bool basePathExist = Directory.Exists(basePath);
                if (!basePathExist)
                {
                    Directory.CreateDirectory(basePath);
                }
                var fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureUrl.FileName);
                fileeName = Path.GetFileName(model.ProfilePictureUrl.FileName);
                var extension = Path.GetExtension(model.ProfilePictureUrl.FileName);
                var filePath = Path.Combine(basePath, model.ProfilePictureUrl.FileName);

                if (!File.Exists(filePath) && extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await model.ProfilePictureUrl.CopyToAsync(stream);
                }
            }
           

            var role = await _roleRepository.GetAsync(staf => staf.Name == "Staff");
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Unable Find Staff Role",
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
            var newSecurity = new Staff
            {
                User = user,
                UserId = user.Id,
                IsDeleted = false,
                ProfilePictureUrl = fileeName,
            };
            await _userRepository.CreateAsync(user);
            user.UserRoles.Add(userRole);
            var staffs = await _staffRepository.CreateAsync(newSecurity);
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
                Message = "Staff created successfully",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateStaffAsync(int id, UpdateStaffRequestModel model)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return new BaseResponse
                {
                    Message = "Staff not found",
                    Success = false
                };
            }
            staff.User.Password = model.Password;
            staff.User.UserName = model.UserName;
            staff.User.PhoneNumber = model.PhoneNumber;
            staff.User.Email = model.Email;
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\ProfilePictures\\");
            bool basePathExist = Directory.Exists(basePath);
            if (!basePathExist)
            {
                Directory.CreateDirectory(basePath);
            }
            var fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureUrl.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePictureUrl.FileName);
            var extension = Path.GetExtension(model.ProfilePictureUrl.FileName);
            if (!File.Exists(filePath) && extension == ".jpg" || extension == ".png" || extension == ".jpeg")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePictureUrl.CopyToAsync(stream);
                }
                staff.ProfilePictureUrl = fileName;
            }
            await _staffRepository.UpdateAsync(staff);
            return new BaseResponse
            {
                Message = "Staff Successfully Updated",
                Success = true
            };
        }


        public async Task<UserImagesResponseModel> GetAllStaffImagesAsync()
        {
            var staffs = await _staffRepository.GetAllAsync();
            if (staffs == null)
            {
                return new UserImagesResponseModel
                {
                    Message = "Staff List is Empty",
                    Success = false
                };
            }
            var imagesUrls = new List<string>();
            foreach (var user in staffs)
            {
                if (user.ProfilePictureUrl != null)
                {
                    imagesUrls.Add(user.ProfilePictureUrl);
                }
            }
            return new UserImagesResponseModel
            {
                Success = true,
                Message = "staffs Images Successfully Retrieved",
                Data = imagesUrls,
            };
        }
    }
}
