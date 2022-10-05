using PrivateEye.Identity;
using PrivateEye.Contracts;
using System.Collections.Generic;
using PrivateEye.Entites;

namespace PrivateEye.Identity
{
    public class User : AuditableEntity
    {
       
        public string UserName { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Password { get; set;}
        public string Email { get; set;}
        public string PhoneNumber { get; set;}
        public Security Security { get; set; }
        public Staff Staff { get; set; }
        public Administrator Administrator { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
