using PrivateEye.Contracts;
using PrivateEye.Entites;
using PrivateEye.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.DTOs
{
    public class CameraDTO : BaseEntity
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string UID { get; set; } 
        public string DeviceType { get; set; }
        public string RegistrationMode { get; set; }
    }
}
