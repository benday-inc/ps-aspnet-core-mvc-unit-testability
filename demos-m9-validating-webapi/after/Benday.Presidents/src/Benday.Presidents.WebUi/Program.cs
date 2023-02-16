using Benday.DataAccess;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Features;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Common;
using Benday.Presidents.WebUi;
using Microsoft.EntityFrameworkCore;
using Benday.Presidents.WebUI.Controllers;
using Benday.Presidents.WebUi.Data;
using Benday.Presidents.WebUi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        (builder.Configuration.GetConnectionString("default"))));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 2;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson()
    .AddXmlSerializerFormatters();

RegisterTypes();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(SecurityConstants.PolicyName_EditPresident,
                      policy => policy.Requirements.Add(
                          new EditPresidentRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, EditPresidentHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UsePopulateSubscriptionClaimsMiddleware();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=President}/{action=Index}/{id?}");

CheckDatabaseHasBeenDeployed();

app.MapRazorPages();

app.Run();

void CheckDatabaseHasBeenDeployed()
{
    using (var scope =
           app.Services
               .CreateScope())
    {
        using (var context = scope.ServiceProvider.GetRequiredService<PresidentsDbContext>())
        {
            context.Database.Migrate();
        }

        using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
        {
            context.Database.Migrate();
        }
    }
}

void RegisterTypes()
{

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();

    builder.Services.AddTransient<IFeatureManager, FeatureManager>();

    builder.Services.AddTransient<Benday.Presidents.Api.Services.ILogger, Logger>();

    builder.Services.AddDbContext<PresidentsDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

    builder.Services.AddTransient<IPresidentsDbContext, PresidentsDbContext>();

    builder.Services.AddTransient<IRepository<Person>, SqlEntityFrameworkPersonRepository>();

    builder.Services.AddTransient<IValidatorStrategy<President>, DefaultValidatorStrategy<President>>();
    builder.Services.AddTransient<IDaysInOfficeStrategy, DefaultDaysInOfficeStrategy>();

    builder.Services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();

    builder.Services.AddTransient<IPresidentService, PresidentService>();
    builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();

    builder.Services.AddTransient<ITestDataUtility, TestDataUtility>();


    builder.Services.AddTransient<PopulateSubscriptionClaimsMiddleware>();

    builder.Services.AddTransient<IUserAuthorizationStrategy, DefaultUserAuthorizationStrategy>();

    builder.Services.AddTransient<IUserClaimsPrincipalProvider,
        HttpContextUserClaimsPrincipalProvider>();


}
