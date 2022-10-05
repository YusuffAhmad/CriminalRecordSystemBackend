using PrivateEye.Model;
using Newtonsoft.Json;
using PrivateEye.DTOs;
using PrivateEye.DTOs.RequestModels;
using PrivateEye.DTOs.ResponseModels;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using PrivateEye.ComparisonModels;
using PrivateEye.EmailServices;

namespace PrivateEye.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IAdministartorRepository _administratorRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStaffService _staffService;
        private readonly IMailServices _mailService;
        private readonly string ngrokPortForwarding = "https://b0be-102-89-41-204.eu.ngrok.io";

        public UserService(IUserRepository userRepository, IStaffService staffService, ISecurityRepository securityRepository, IAdministartorRepository administratorRepository, IMailServices mailService)
        {
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
            _staffService = staffService;
            _mailService = mailService;
            _securityRepository = securityRepository;
        }

        public async Task<BaseResponse> DeleteUserAsync(int id)
        {
            var user = await _administratorRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (user == null )
            {
                return new BaseResponse
                {
                    Message = "User not found",
                    Success = false
                };
            }

            user.IsDeleted = true;
            await _administratorRepository.UpdateAsync(user);
            return new BaseResponse
            {
                Message = "Users Successfully Updated",
                Success = true
            };
        }

        public async Task<UsersResponseModel> GetAllUserAsync()
        {
            var user = await _userRepository.GetAllAsync();
            if (user == null)
            {
                return new UsersResponseModel
                {
                    Message = "User List is Empty",
                    Success = false
                };
            }
            return new UsersResponseModel
            {
                Success = true,
                Message = "Users Successfully Retrieved",
                Data = user.Select(user => new UserDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }).ToList(),
            };
        }

        public async Task<UserResponseModel> GetUserAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null && user.IsDeleted == true)
            {
                return new UserResponseModel
                {
                    Message = "Role not found",
                    Success = false
                };
            }
            return new UserResponseModel
            {
                Success = true,
                Message = "Role Successfully Retrieved",
                Data = new UserDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    Password = user.Password,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                },
            };
        }

        public async Task<UserResponseModel> LoginAsync(UserLoginRequestModelModel model)
        {
            var user = await _userRepository.ExistsByEmailAsync(model.Email, model.Password);
            if (user == null)
            {
                return new UserResponseModel
                {
                    Message = "Invalid Username or password",
                    Success = false,
                };
            }
            var verifyPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (verifyPassword == false)
            {
                return new UserResponseModel
                {
                    Message = "Invalid Username or password",
                    Success = false,
                };
            }
            return new UserResponseModel
            {
                Message = "User successfully Logged In",
                Success = true,
                Data = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRoles = user.UserRoles.Select(role => new RoleDTO
                    {
                        Id = role.Role.Id,
                        Name = role.Role.Name,
                        Description = role.Role.Description
                    }).ToList()
                },
                
            };
        }

        public string SaveDataUrlAsImage(string dataUrl)
        {
            string path;
            string filename;

            var base64Data = Regex.Match(dataUrl, @"data:image/(?<Type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Intruders\\");
            bool exist = Directory.Exists(path);
            if (!exist)
            {
                Directory.CreateDirectory(path);
            }

            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            string fileName = "Sn" + milliseconds.ToString() + ".jpg";
            filename = Path.Combine(path, fileName);

            if (!File.Exists(filename))
            {
                File.WriteAllBytes(filename, binData);
            }
            return fileName;
        }


        public async Task<string> GetFaceIDAsync(string imageUrl)
        {
            string endpoint_url = "https://api.imagga.com/v2/faces/detections";

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(endpoint_url)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YWNjX2I4MDlkNTE3OWFiMGM4ZTo5ZmVlZGFmYmQ1ZTEzNGY0NjQ3OTI0MWZkMGNlZWNiYQ==");
            var Content = endpoint_url + "?image_url=" + imageUrl + "&return_face_id=" + 1;
            HttpResponseMessage response = await httpClient.GetAsync(Content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                FaceIDResponse compareFaces = JsonConvert.DeserializeObject<FaceIDResponse>(apiResponse);
                return compareFaces.Result.Faces.Select(x => x.Face_id).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public async Task<BaseResponse> VerifyAsync(string imageUrl)
        {
            string endpoint_url = "https://api.imagga.com/v2/faces/similarity";
            var allAdmin = await _administratorRepository.GetAdministratorsAsync();
            var allSecurity = await _securityRepository.GetAllSecuritiesAsync();
            List<decimal> scores = new List<decimal>();
            var fileName = SaveDataUrlAsImage(imageUrl);
            var userImagesUrl = await _staffService.GetAllStaffImagesAsync();

            foreach (var image in userImagesUrl.Data)
            {
                string face_id = await GetFaceIDAsync($"{ngrokPortForwarding}/Intruders/" + fileName);
                string second_face_id = await GetFaceIDAsync($"{ngrokPortForwarding}/ProfilePictures/" + image);

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(endpoint_url)
                };
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YWNjX2I4MDlkNTE3OWFiMGM4ZTo5ZmVlZGFmYmQ1ZTEzNGY0NjQ3OTI0MWZkMGNlZWNiYQ==");
                string content = "?face_id=" + face_id + "&second_face_id=" + second_face_id;
                HttpResponseMessage response = await httpClient.GetAsync(endpoint_url + content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    CompareResponse compareFaces = JsonConvert.DeserializeObject<CompareResponse>(apiResponse);
                    scores.Add(compareFaces.Result.score);
                }
            }
            if (scores.Count <= 0)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Error proccessing the request"
                };
            }
            scores.Sort();
            var largestScore = scores[^1];
           
            if (largestScore > 70)
            {
                foreach (var admin in allAdmin)
                {
                    var mailRequest = new MailRequest
                    {
                        Subject = "Identification of Staff With Criminal Records",
                        ToEmail = admin.User.Email,
                        ToName = admin.User.FirstName + admin.User.LastName,
                        HtmlContent = $"<html><body><h1>Hello {admin.User.FirstName + admin.User.LastName}, A Staff with criminal record has been identified, please take action </h1></body></html>",
                        AttachmentName = "Culprit.txt"
                    };
                    _mailService.SendEMailAsync(mailRequest);
                }
                foreach (var security in allSecurity)
                {
                    var mailRequest = new MailRequest
                    {
                        Subject = "Identification of Staff With Criminal Records",
                        ToEmail = security.User.Email,
                        ToName = security.User.FirstName + security.User.LastName,
                        HtmlContent = $"<html><body><h1>Hello {security.User.FirstName + security.User.LastName}, A Staff with criminal record has been identified, please take action </h1></body></html>",
                        AttachmentName = "Culprit"
                    };
                    _mailService.SendEMailAsync(mailRequest);
                }
                return new BaseResponse
                {
                    Success = true,
                    Message = "Criminal Record found"
                };
            }
            else
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "No Criminal Records"
                };
            }
        }
    }
}
