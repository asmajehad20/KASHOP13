using KASHOP13.BLL.Service;
using KASHOP13.DAL.Repository;
using KASHOP13.DAL.Utility;

namespace KASHOP13.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();

            Services.AddScoped<IBrandRepository, BrandRepository>();
            Services.AddScoped<IBrandService, BrandService>();

            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IProductService, ProductService>();

            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICartService, CartService>();

            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
            return Services;
        }
    }
}
