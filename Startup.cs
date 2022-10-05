using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrivateEye.Auth;
using PrivateEye.Context;
using PrivateEye.EmailServices;
using PrivateEye.Implementations.Repositories;
using PrivateEye.Implementations.Services;
using PrivateEye.InterFaces.IRepositories;
using PrivateEye.InterFaces.IServices;

namespace PrivateEye
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c => c
                .AddPolicy("PrivateEye", builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()));

            services.AddDbContext<ApplicationContext>(options => options
            .UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IAdministartorRepository, AdministratorRepository>();
            
            services.AddScoped<ICameraService, CameraService>();
            services.AddScoped<ICameraRepository, CameraRepository>();
            
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ISecurityRepository, SecurityRepository>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IMailServices, MailService>();

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrivateEye", Version = "v1"});
            });

            var key = "5f00cce7-5341-4d47-bed9-f878ac54dec2";
            services.AddSingleton<IJWTAuthenticationManager>(new JWTAuthenticationManager(key));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.RequireAuthenticatedSignIn = true;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Private Eye",
                    ValidAudience = "Private Eye Users",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
                options.SaveToken = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrivateEye v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("PrivateEye");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrivateEye v1");
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
