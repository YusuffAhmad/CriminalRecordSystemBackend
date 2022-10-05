using Microsoft.EntityFrameworkCore;
using PrivateEye.Entites;
using PrivateEye.Identity;

namespace PrivateEye.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Staff> Staffs { get; set; }
    }
}