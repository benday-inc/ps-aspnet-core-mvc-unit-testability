using Benday.DataAccess;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Features;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

RegisterTypes();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=President}/{action=Index}/{id?}");

CheckDatabaseHasBeenDeployed();

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

    // builder.Services.AddTransient<IValidatorStrategy<President>, DefaultValidatorStrategy<President>>();

    builder.Services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();

    builder.Services.AddTransient<IPresidentService, PresidentService>();
}