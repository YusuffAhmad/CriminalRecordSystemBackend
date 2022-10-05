namespace PrivateEye.DTOs.RequestModels
{
    public class UpdateCameraRequestModel
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string UID { get; set; }
        public string DeviceType { get; set; }
        public string RegistrationMode { get; set; }
    }
}
