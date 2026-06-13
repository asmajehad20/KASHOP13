using KASHOP13.BLL.Mapping;
using KASHOP13.DAL.Utility;
using KASHOP13.PL.Extentions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCoresPolicyServices();

builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddLocalizationServices();
builder.Services.AddIdentityServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddApplicationServices();

MapsterConfig.MapsterConfigRegister();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

HttpContextHelper.Accessor =
    app.Services.GetRequiredService<IHttpContextAccessor>();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(AddCoresPolicyExtentions.PolicyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeders = services.GetServices<ISeedData>();
    foreach(var seeder in seeders)
    {
        await seeder.DataSeed();
    }
}

app.Run();
