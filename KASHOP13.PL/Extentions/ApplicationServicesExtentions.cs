using KASHOP13.BLL.Service;
using KASHOP13.DAL.Repository;
using KASHOP13.DAL.Utility;
using Stripe;

namespace KASHOP13.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();

            Services.AddScoped<IBrandRepository, BrandRepository>();
            Services.AddScoped<IBrandService, BrandService>();

            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IProductService, BLL.Service.ProductService>();

            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICartService, CartService>();

            Services.AddScoped<IFileService, BLL.Service.FileService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();

            Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            Services.AddScoped<ICheckoutService, BLL.Service.CheckoutService>();
            Services.AddScoped<IOrderRepository, OrderRepository>();

            return Services;
        }
    }
}
