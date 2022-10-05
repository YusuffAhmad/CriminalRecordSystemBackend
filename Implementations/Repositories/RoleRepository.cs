using PrivateEye.Context;
using PrivateEye.Identity;
using PrivateEye.Implementation.Repositries;
using PrivateEye.InterFaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationContext Context)
        {
            _Context = Context;
        }
    }
}
