using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Enitities;

namespace Dating.API.Extension
{
    public static class DbRegisteredExtension
    {
        public static void ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DatingSiteContext>().AddDefaultTokenProviders();
            services.AddDbContext<DatingSiteContext>(dbContextOptions => dbContextOptions.UseNpgsql(configuration["ConnectionStrings:ProdDb"]));
        }
    }
}