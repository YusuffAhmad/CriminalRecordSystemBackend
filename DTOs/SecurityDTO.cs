using PrivateEye.Contracts;
using PrivateEye.Entites;
using PrivateEye.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs
{
    public class SecurityDTO : BaseEntity
    {
        public string AgencyName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
