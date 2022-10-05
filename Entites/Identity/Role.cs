using PrivateEye.Identity;
using PrivateEye.Contracts;
using System.Collections.Generic;

namespace PrivateEye.Identity
{
    public class Role : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
