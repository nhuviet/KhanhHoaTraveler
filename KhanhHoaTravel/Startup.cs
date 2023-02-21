using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace KhanhHoaTravel
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
            //services.AddAuthentication().AddGoogle(options => {
            //    var googleAuthNSection = Configuration.GetSection("Authentication:Google");
            //    options.ClientId = googleAuthNSection["ClientId"];
            //    options.ClientSecret = googleAuthNSection["ClientSecret"];
            //    //options.CallbackPath = "/OutLog";
            //    options.CallbackPath = "/signin-google";
            //    options.Scope.Add("email");
            //    options.Scope.Add("profile");
            //});
            //services.AddIdentity<_User, IdentityRole>().AddSignInManager<SignInManager<_User>>();
            //services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddIdentity<_User>().AddSignInManager<SignInManager<_User>>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                var googleAuthNSection = Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });
            services.AddControllersWithViews();
            services.AddDbContext<KHTravelDbContext>(opts =>
            {
                opts.UseSqlServer(
                    Configuration["ConnectionStrings:DataConnect"]);
            });
            //services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddScoped<IKHTravelRepository, EFKHTravelRepository>();
            services.AddSession();
            //services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Service}/{action=PlaceDetail}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=ChangeStatusUser}/{id}/{status}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=AccountDetail}/{id?}");
                //endpoints.MapControllerRoute(
                //    name: "signin-google",
                //    pattern: "signin-google",
                //    defaults: new { controller = "Authentication", action = "ExternalLoginCallback" });
                endpoints.MapControllerRoute(
                    name: "signin-google",
                    pattern: "signin-google",
                    defaults: new { controller = "Authentication", action = "GoogleResponse" });
                //endpoints.MapRazorPages();
                //endpoints.MapBlazorHub();
                //endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
