using PrivateEye.Contracts;
using PrivateEye.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Entites
{
    public class Staff : AuditableEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
