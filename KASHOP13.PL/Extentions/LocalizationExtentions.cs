using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace KASHOP13.PL.Extentions
{
    public static class LocalizationExtentions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection Services)
        {
            Services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar"),
                //new CultureInfo("fr")
            };
            Services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });
            return Services;
        }
    }
}
