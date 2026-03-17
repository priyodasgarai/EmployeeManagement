using Employee.Data;
using Employee.Interfaces;
using Employee.Middlewares;
using Employee.Models;
using Employee.Repository;
using Employee.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Employee.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration Configuration)
        {
            string connectionString = "DefaultConnection";
            services.AddDbContext<ApplicationDBContext>(Options => Options.
            UseSqlServer(Configuration.GetConnectionString(connectionString),
                        sqlOptions =>
                        {
                            //Ensure this is the correct assemble
                            sqlOptions.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName);
                            sqlOptions.EnableRetryOnFailure(); //Enaple automatic retries for transient failures
                        })
                        .EnableSensitiveDataLogging()
                        // .UseExceptionProcessor()
                        , ServiceLifetime.Scoped);
            return services;
        }
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ISqlDataAccess, SqlDataAccess>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //For Identity 
            services.AddIdentityCore<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            }).AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDBContext>();


            //Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                 options.DefaultForbidScheme =
                options.DefaultScheme =
                 options.DefaultSignInScheme =
                 options.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
            })//Adding JWT Bearer
            .AddJwtBearer(Options =>
            {
                Options.SaveToken = true;
                Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (
                        Encoding.UTF8.GetBytes(Configuration["JWT:Signingkey"])
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
        public static IApplicationBuilder UseInfrastructructureService(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }






    }
}
