using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TriviaQuizMVC.Data;
using TriviaQuizMVC.Services;

namespace TriviaQuizMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // MIGRATIONS HANDLING
            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<TriviaDbContext>();
            //    var migrations = context.Database.GetPendingMigrations().ToList();
            //    if (migrations.Any())
            //    {
            //        context.Database.Migrate();
            //    }
            //}

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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<TriviaDbContext>();
                //options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                //options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<TriviaDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddSingleton<StashQuizService>();
        }
    }
}