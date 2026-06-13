using KASHOP13.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace KASHOP13.PL.Extentions
{
    public static class DatabaseExtentions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            return Services;
        }
    }
}
