using PrivateEye.Entites;
using PrivateEye.Implementation.Repositries;
using PrivateEye.Interface.IRespositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IRepositories
{
    public interface IAdministartorRepository : IBaseRepository<Administrator>
    {
        Task<List<Administrator>> GetAdministratorsAsync();
        Task<Administrator> GetAdminByIdAsync(int id);
    }
}
