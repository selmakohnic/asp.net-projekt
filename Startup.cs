using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using smalandscamping.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace smalandscamping
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(services).Wait();
        }

        //Kod för att skapa användarroller
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Kontrollerar om det inte finns någon användare med e-postadressen nedan
            if (await _userManager.FindByEmailAsync("admin@smalandscamping.se") == null)
            {
                //Skapar en ny användare som är admin
                IdentityUser administrator = new IdentityUser()
                {
                    UserName = "admin@smalandscamping.se",
                    Email = "admin@smalandscamping.se",
                    EmailConfirmed = true
                };

                //Lösenord för inloggning för admin
                await _userManager.CreateAsync(administrator, "Lösenord1");

                //Skapar användarrollen admin
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

                IdentityResult result = await _userManager.AddToRoleAsync(administrator, "Admin");
            }
        }
    }
}