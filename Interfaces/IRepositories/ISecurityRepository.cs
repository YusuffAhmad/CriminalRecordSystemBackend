using PrivateEye.Entites;
using PrivateEye.Interface.IRespositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IRepositories
{
    public interface ISecurityRepository : IBaseRepository<Security>
    {
        Task<ICollection<Security>> GetAllSecuritiesAsync();
        Task<Security> GetSecurityByIdAsync(int id);
    }
}
