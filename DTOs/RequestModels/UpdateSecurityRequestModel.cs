using Microsoft.AspNetCore.Http;

namespace PrivateEye.DTOs.RequestModels
{
    public class UpdateSecurityRequestModel
    {
        public string AgencyName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}
