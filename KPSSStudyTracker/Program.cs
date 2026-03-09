using KPSSStudyTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace KPSSStudyTracker;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
        builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
            });
        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        // Request localization to Turkish
        var tr = new CultureInfo("tr-TR");
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(tr),
            SupportedCultures = new List<CultureInfo> { tr },
            SupportedUICultures = new List<CultureInfo> { tr }
        };
        app.UseRequestLocalization(localizationOptions);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        // Ensure database is created and migrations are applied
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                // Apply pending migrations automatically
                await dbContext.Database.MigrateAsync();
                
                // Seed initial data
                await Data.SeedData.InitializeAsync(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                // Log error but don't crash the app
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating or seeding the database. Message: {Message}, InnerException: {InnerException}", 
                    ex.Message, 
                    ex.InnerException?.Message ?? "None");
                
                // Log full exception details in development
                if (app.Environment.IsDevelopment())
                {
                    logger.LogError(ex, "Full exception: {Exception}", ex.ToString());
                }
            }
        }

        app.Run();
    }
}
