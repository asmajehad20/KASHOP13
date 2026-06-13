namespace KASHOP13.PL.Extentions
{
    public static class AddCoresPolicyExtentions
    {
        public const string PolicyName = "_myAllowSpecificOrigins";
        public static IServiceCollection AddCoresPolicyServices(this IServiceCollection Services)
        {
            
            Services.AddCors(options =>
            {
                options.AddPolicy(name: PolicyName,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            return Services;
        }
    }
}
