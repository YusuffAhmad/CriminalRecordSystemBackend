using PrivateEye.DTOs.RequestModels;

namespace PrivateEye.EmailServices
{
    public interface IMailServices
    {
       public void SendEMailAsync(MailRequest mailRequest);
    }
}
