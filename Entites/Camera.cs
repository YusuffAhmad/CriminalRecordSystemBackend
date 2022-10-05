using PrivateEye.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Entites
{
    public class Camera : AuditableEntity
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string UID { get; set; } 
        public string DeviceType { get; set; }
        public string RegistrationMode { get; set; }
    }
}
