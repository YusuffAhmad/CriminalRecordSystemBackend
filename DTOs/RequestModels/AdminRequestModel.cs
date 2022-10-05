using Microsoft.AspNetCore.Http;

namespace PrivateEye.DTOs.RequestModels
{
    public class AdministratorRequestModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}
