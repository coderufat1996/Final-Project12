using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.DAL;
using OnlineShoppingApp.DAL.Entities;
using OnlineShoppingApp.Options;
using OnlineShoppingApp.Servıces;

namespace OnlineShoppingApp
{
    public static class RegisterService
    {
        public static void Register(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddMvc();
            services.AddControllersWithViews();

            services.AddDbContext<OnlineShoppingDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<EmailConfigOptions>(configuration.GetSection(nameof(EmailConfigOptions)));

            services.AddScoped<IEmailService, EmailService>();

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredUniqueChars = 3;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.User.RequireUniqueEmail = true;
                //opt.User.AllowedUserNameCharacters = default;

                //opt.SignIn.RequireConfirmedEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.AllowedForNewUsers = true;


            }).AddDefaultTokenProviders().AddEntityFrameworkStores<OnlineShoppingDbContext>();
        }
    }
}
